using System;
using ApacheTech.VintageMods.NeverWinter.Settings;
using Gantry.Core.Hosting.Annotation;

namespace ApacheTech.VintageMods.NeverWinter.Commands;

[UsedImplicitly]
internal class NeverWinterServerCommand
{
    private readonly ICoreServerAPI _sapi;
    private readonly IServerNetworkChannel _serverChannel;
    private readonly NeverWinterSettings _settings;

    [SidedConstructor(EnumAppSide.Server)]
    public NeverWinterServerCommand(ICoreServerAPI sapi, NeverWinterSettings settings, IServerNetworkService serverNetworkService)
    {
        _settings = settings;
        _serverChannel = serverNetworkService
            .DefaultServerChannel
            .RegisterMessageType<NeverWinterSettingsPacket>();

        (_sapi = sapi).Event.PlayerJoin += player =>
            _serverChannel.SendPacket(_settings.ToPacket(), player);
    }

    public void GenerateCommand()
    {
        var command = _sapi.ChatCommands
            .Create("nwn")
            .WithDescription(T("OnDisplaySettings.Description"))
            .RequiresPrivilege(Privilege.controlserver)
            .HandleWith(OnDisplaySettings);

        command.BeginSubCommand("hibernate")
            .WithDescription(T("OnHibernateSettingsChange.Description"))
            .WithArgs(
                new BoolArgParser("Enabled", "enabled", true),
                new WordRangeArgParser(T("StartMonth"), false, Enum.GetNames(typeof(EnumMonth))),
                new WordRangeArgParser(T("EndMonth"), false, Enum.GetNames(typeof(EnumMonth))))
            .HandleWith(OnHibernateSettingsChange)
            .EndSubCommand();

        command.BeginSubCommand("min")
            .WithDescription(T("OnMinimumTemperatureChange.Description"))
            .WithArgs(new FloatArgParser(T("MinimumTemperature"), -100f, 100f, true))
            .HandleWith(OnMinimumTemperatureChange)
            .EndSubCommand();

        command.BeginSubCommand("max")
            .WithDescription(T("OnMaximumTemperatureChange.Description"))
            .WithArgs(new FloatArgParser(T("MaximumTemperature"), -100f, 100f, true))
            .HandleWith(OnMaximumTemperatureChange)
            .EndSubCommand();

        command.BeginSubCommand("season")
            .WithDescription(T("OnOverrideSeasonChange.Description"))
            .WithArgs(new WordRangeArgParser(T("Season"), true, "spring", "summer", "autumn", "winter", "auto"))
            .HandleWith(OnOverrideSeasonChange)
            .EndSubCommand();

        command.BeginSubCommand("exclude")
            .WithDescription(T("OnExcludeSeasonChange.Description"))
            .WithArgs(new WordRangeArgParser(T("Season"), true, "spring", "summer", "autumn", "winter", "none"))
            .HandleWith(OnExcludeSeasonChange)
            .EndSubCommand();

        command.BeginSubCommand("reset")
            .WithDescription(T("OnReset.Description"))
            .HandleWith(OnReset)
            .EndSubCommand();
    }

    private TextCommandResult OnDisplaySettings(TextCommandCallingArgs args)
    {
        var sb = new StringBuilder();
        sb.AppendLine(T("OnHibernateSettingsChange.HibernationEnabled.Feedback", _settings.HibernationEnabled ? "enabled" : "disabled"));
        if (_settings.HibernationEnabled)
        {
            sb.AppendLine(T("OnHibernateSettingsChange.HibernationStartMonth.Feedback", _settings.HibernationStartMonth));
            sb.AppendLine(T("OnHibernateSettingsChange.HibernationEndMonth.Feedback", _settings.HibernationEndMonth));
        }
        sb.AppendLine(T("OnMinimumTemperatureChange.Feedback", _settings.MinTemperature));
        sb.AppendLine(T("OnMaximumTemperatureChange.Feedback", _settings.MaxTemperature));
        sb.AppendLine(T("OnOverrideSeasonChange.Feedback", _settings.SeasonOverride.UcFirst()));
        sb.AppendLine(T("OnExcludeSeasonChange.Feedback", _settings.ExcludeSeason.UcFirst()));
        return TextCommandResult.Success(sb.ToString());
    }

    private TextCommandResult OnHibernateSettingsChange(TextCommandCallingArgs args)
    {
        _settings.HibernationEnabled = args[0].To<bool>();
        _settings.HibernationStartMonth = args[1].To<string>().IfNullOrWhitespace("November");
        _settings.HibernationEndMonth = args[2].To<string>().IfNullOrWhitespace("April");

        ModSettings.World.Save(_settings);
        _serverChannel.BroadcastPacket(_settings.ToPacket());

        var sb = new StringBuilder();
        sb.AppendLine(T("OnHibernateSettingsChange.HibernationEnabled.Feedback", _settings.HibernationEnabled ? "Enabled" : "Disabled"));
        if (!_settings.HibernationEnabled) return TextCommandResult.Success(sb.ToString());
        sb.AppendLine(T("OnHibernateSettingsChange.HibernationStartMonth.Feedback", _settings.HibernationStartMonth));
        sb.AppendLine(T("OnHibernateSettingsChange.HibernationEndMonth.Feedback", _settings.HibernationEndMonth));
        return TextCommandResult.Success(sb.ToString());
    }

    private TextCommandResult OnMinimumTemperatureChange(TextCommandCallingArgs args)
    {
        _settings.MinTemperature = args[0].To<float>();
        ModSettings.World.Save(_settings);
        _serverChannel.BroadcastPacket(_settings.ToPacket());
        return TextCommandResult.Success(T("OnMinimumTemperatureChange.Feedback", _settings.MinTemperature));
    }

    private TextCommandResult OnMaximumTemperatureChange(TextCommandCallingArgs args)
    {
        _settings.MaxTemperature = args[0].To<float>();
        ModSettings.World.Save(_settings);
        _serverChannel.BroadcastPacket(_settings.ToPacket());
        return TextCommandResult.Success(T("OnMaximumTemperatureChange.Feedback", _settings.MaxTemperature));
    }

    private TextCommandResult OnOverrideSeasonChange(TextCommandCallingArgs args)
    {
        _settings.SeasonOverride = args[0].ToString();
        ModSettings.World.Save(_settings);
        _sapi.World.Calendar.SetSeasonOverride(_settings.SeasonOverride);
        _serverChannel.BroadcastPacket(_settings.ToPacket());
        return TextCommandResult.Success(T("OnOverrideSeasonChange.Feedback", _settings.SeasonOverride.UcFirst()));
    }

    private TextCommandResult OnExcludeSeasonChange(TextCommandCallingArgs args)
    {
        _settings.ExcludeSeason = args[0].ToString();
        ModSettings.World.Save(_settings);
        _serverChannel.BroadcastPacket(_settings.ToPacket());
        return TextCommandResult.Success(T("OnExcludeSeasonChange.Feedback", _settings.ExcludeSeason.UcFirst()));
    }

    private TextCommandResult OnReset(TextCommandCallingArgs _)
    {
        _settings.Reset();
        ModSettings.World.Save(_settings);
        _sapi.World.Calendar.SetSeasonOverride(null);
        _serverChannel.BroadcastPacket(_settings.ToPacket());
        return TextCommandResult.Success(T("OnReset.Feedback"));
    }

    private static string T(string key, params object[] args)
        => LangEx.FeatureString("NeverWinter.ServerCommand", key, args);
}
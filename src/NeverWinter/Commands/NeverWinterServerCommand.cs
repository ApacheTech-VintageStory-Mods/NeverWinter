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
            .WithDescription(LangEntry("OnDisplaySettings.Description"))
            .RequiresPrivilege(Privilege.controlserver)
            .HandleWith(OnDisplaySettings);

        command.BeginSubCommand("hibernate")
            .WithDescription(LangEntry("OnHibernateSettingsChange.Description"))
            .WithArgs(
                new BoolArgParser("Enabled", "enabled", true),
                new WordRangeArgParser(LangEntry("StartMonth"), false, Enum.GetNames(typeof(EnumMonth))),
                new WordRangeArgParser(LangEntry("EndMonth"), false, Enum.GetNames(typeof(EnumMonth))))
            .HandleWith(OnHibernateSettingsChange)
            .EndSubCommand();

        command.BeginSubCommand("min")
            .WithDescription(LangEntry("OnMinimumTemperatureChange.Description"))
            .WithArgs(new FloatArgParser(LangEntry("MinimumTemperature"), -100f, 100f, true))
            .HandleWith(OnMinimumTemperatureChange)
            .EndSubCommand();

        command.BeginSubCommand("max")
            .WithDescription(LangEntry("OnMaximumTemperatureChange.Description"))
            .WithArgs(new FloatArgParser(LangEntry("MaximumTemperature"), -100f, 100f, true))
            .HandleWith(OnMaximumTemperatureChange)
            .EndSubCommand();

        command.BeginSubCommand("season")
            .WithDescription(LangEntry("OnOverrideSeasonChange.Description"))
            .WithArgs(new WordRangeArgParser(LangEntry("Season"), true, "spring", "summer", "autumn", "winter", "auto"))
            .HandleWith(OnOverrideSeasonChange)
            .EndSubCommand();

        command.BeginSubCommand("exclude")
            .WithDescription(LangEntry("OnExcludeSeasonChange.Description"))
            .WithArgs(new WordRangeArgParser(LangEntry("Season"), true, "spring", "summer", "autumn", "winter", "none"))
            .HandleWith(OnExcludeSeasonChange)
            .EndSubCommand();

        command.BeginSubCommand("reset")
            .WithDescription(LangEntry("OnReset.Description"))
            .HandleWith(OnReset)
            .EndSubCommand();
    }

    private TextCommandResult OnDisplaySettings(TextCommandCallingArgs args)
    {
        var sb = new StringBuilder();
        sb.AppendLine(LangEntry("OnHibernateSettingsChange.HibernationEnabled.Feedback", _settings.HibernationEnabled ? "enabled" : "disabled"));
        if (_settings.HibernationEnabled)
        {
            sb.AppendLine(LangEntry("OnHibernateSettingsChange.HibernationStartMonth.Feedback", _settings.HibernationStartMonth));
            sb.AppendLine(LangEntry("OnHibernateSettingsChange.HibernationEndMonth.Feedback", _settings.HibernationEndMonth));
        }
        sb.AppendLine(LangEntry("OnMinimumTemperatureChange.Feedback", _settings.MinTemperature));
        sb.AppendLine(LangEntry("OnMaximumTemperatureChange.Feedback", _settings.MaxTemperature));
        sb.AppendLine(LangEntry("OnOverrideSeasonChange.Feedback", _settings.SeasonOverride.UcFirst()));
        sb.AppendLine(LangEntry("OnExcludeSeasonChange.Feedback", _settings.ExcludeSeason.UcFirst()));
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
        sb.AppendLine(LangEntry("OnHibernateSettingsChange.HibernationEnabled.Feedback", _settings.HibernationEnabled ? "Enabled" : "Disabled"));
        if (!_settings.HibernationEnabled) return TextCommandResult.Success(sb.ToString());
        sb.AppendLine(LangEntry("OnHibernateSettingsChange.HibernationStartMonth.Feedback", _settings.HibernationStartMonth));
        sb.AppendLine(LangEntry("OnHibernateSettingsChange.HibernationEndMonth.Feedback", _settings.HibernationEndMonth));
        return TextCommandResult.Success(sb.ToString());
    }

    private TextCommandResult OnMinimumTemperatureChange(TextCommandCallingArgs args)
    {
        _settings.MinTemperature = args[0].To<float>();
        ModSettings.World.Save(_settings);
        _serverChannel.BroadcastPacket(_settings.ToPacket());
        return TextCommandResult.Success(LangEntry("OnMinimumTemperatureChange.Feedback", _settings.MinTemperature));
    }

    private TextCommandResult OnMaximumTemperatureChange(TextCommandCallingArgs args)
    {
        _settings.MaxTemperature = args[0].To<float>();
        ModSettings.World.Save(_settings);
        _serverChannel.BroadcastPacket(_settings.ToPacket());
        return TextCommandResult.Success(LangEntry("OnMaximumTemperatureChange.Feedback", _settings.MaxTemperature));
    }

    private TextCommandResult OnOverrideSeasonChange(TextCommandCallingArgs args)
    {
        _settings.SeasonOverride = args[0].ToString();
        ModSettings.World.Save(_settings);
        _sapi.World.Calendar.SetSeasonOverride(_settings.SeasonOverride);
        _serverChannel.BroadcastPacket(_settings.ToPacket());
        return TextCommandResult.Success(LangEntry("OnOverrideSeasonChange.Feedback", _settings.SeasonOverride.UcFirst()));
    }

    private TextCommandResult OnExcludeSeasonChange(TextCommandCallingArgs args)
    {
        _settings.ExcludeSeason = args[0].ToString();
        ModSettings.World.Save(_settings);
        _serverChannel.BroadcastPacket(_settings.ToPacket());
        return TextCommandResult.Success(LangEntry("OnExcludeSeasonChange.Feedback", _settings.ExcludeSeason.UcFirst()));
    }

    private TextCommandResult OnReset(TextCommandCallingArgs _)
    {
        _settings.Reset();
        ModSettings.World.Save(_settings);
        _sapi.World.Calendar.SetSeasonOverride(null);
        _serverChannel.BroadcastPacket(_settings.ToPacket());
        return TextCommandResult.Success(LangEntry("OnReset.Feedback"));
    }

    private static string LangEntry(string key, params object[] args)
        => LangEx.FeatureString("NeverWinter.ServerCommand", key, args);
}
namespace ApacheTech.VintageMods.NeverWinter;

internal sealed class NeverWinterServerCommand
{
    private readonly ICoreServerAPI _sapi;
    private readonly IServerNetworkChannel _serverChannel;
    private NeverWinterSettings _settings;

    public NeverWinterServerCommand(ICoreServerAPI sapi)
    {
        _settings = ModSettings.World.Feature<NeverWinterSettings>();
        _serverChannel = IOC.Services
            .Resolve<IServerNetworkService>()
            .DefaultServerChannel
            .RegisterMessageType<NeverWinterSettings>();

        (_sapi = sapi).Event.PlayerJoin += player =>
            _serverChannel.SendPacket(_settings, player);
    }

    public void Generate()
    {
        var command = _sapi.ChatCommands
            .Create("nwn")
            .WithDescription(LangEntry("OnDisplaySettings.Description"))
            .RequiresPrivilege(Privilege.controlserver)
            .HandleWith(OnDisplaySettings);

        command.BeginSubCommand("min")
            .WithDescription(LangEntry("OnMinimumTemperatureChange.Description"))
            .WithArgs(new FloatArgParser(LangEntry("MinimumTemperature"), -20f, 40f, true))
            .HandleWith(OnMinimumTemperatureChange)
            .EndSubCommand();

        command.BeginSubCommand("max")
            .WithDescription(LangEntry("OnMaximumTemperatureChange.Description"))
            .WithArgs(new FloatArgParser(LangEntry("MaximumTemperature"), -20f, 40f, true))
            .HandleWith(OnMaximumTemperatureChange)
            .EndSubCommand();

        command.BeginSubCommand("season")
            .WithDescription(LangEntry("OnOverrideSeasonChange.Description"))
            .WithArgs(new WordRangeArgParser(LangEntry("Season"), true, "spring", "summer", "autumn", "winter", "auto"))
            .HandleWith(OnOverrideSeasonChange)
            .EndSubCommand();

        command.BeginSubCommand("reset")
            .WithDescription(LangEntry("OnReset.Description"))
            .HandleWith(OnReset)
            .EndSubCommand();
    }

    private TextCommandResult OnDisplaySettings(TextCommandCallingArgs args)
    {
        var sb = new StringBuilder();
        sb.AppendLine(LangEntry("OnMinimumTemperatureChange.Feedback", _settings.MinTemperature));
        sb.AppendLine(LangEntry("OnMaximumTemperatureChange.Feedback", _settings.MaxTemperature));
        sb.AppendLine(LangEntry("OnOverrideSeasonChange.Feedback", _settings.SeasonOverride.UcFirst()));
        return TextCommandResult.Success(sb.ToString());
    }

    private TextCommandResult OnMinimumTemperatureChange(TextCommandCallingArgs args)
    {
        _settings.MinTemperature = args[0].To<float>();
        _serverChannel.BroadcastPacket(_settings);
        return TextCommandResult.Success(LangEntry("OnMinimumTemperatureChange.Feedback", _settings.MinTemperature));
    }

    private TextCommandResult OnMaximumTemperatureChange(TextCommandCallingArgs args)
    {
        _settings.MaxTemperature = args[0].To<float>();
        _serverChannel.BroadcastPacket(_settings);
        return TextCommandResult.Success(LangEntry("OnMaximumTemperatureChange.Feedback", _settings.MaxTemperature));
    }

    private TextCommandResult OnOverrideSeasonChange(TextCommandCallingArgs args)
    {
        _settings.SeasonOverride = args[0].ToString();
        _sapi.World.Calendar.SetSeasonOverride(_settings.SeasonOverride);
        return TextCommandResult.Success(LangEntry("OnOverrideSeasonChange.Feedback", _settings.SeasonOverride.UcFirst()));
    }

    private TextCommandResult OnReset(TextCommandCallingArgs _)
    {
        _settings = NeverWinterSettings.Default;
        ModSettings.World.Save(_settings);
        _sapi.World.Calendar.SetSeasonOverride(null);
        _serverChannel.BroadcastPacket(_settings);
        return TextCommandResult.Success(LangEntry("OnReset.Feedback"));
    }

    private static string LangEntry(string key, params object[] args)
        => LangEx.FeatureString("NeverWinter.ServerCommand", key, args);
}
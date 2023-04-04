﻿using Development.Suite.Logging;
using System.Diagnostics;

namespace Development.Suite.Plugin.ConsoleRunner;

public class ConsoleMessageHandler : IMessageHandler<ConsoleMessage>
{
    private readonly IDevelopmentSuiteLogger<ConsoleMessageHandler> _logger;
    private readonly IIpcMessageSender _ipcMessageSender;

    public ConsoleMessageHandler(IDevelopmentSuiteLogger<ConsoleMessageHandler> logger, IIpcMessageSender ipcMessageSender)
    {
        _logger = logger;
        _ipcMessageSender = ipcMessageSender;
    }

    public void HandleMessage(ConsoleMessage message)
    {
        _ipcMessageSender.SendMessage(new ConsoleMessage(message)
        {
            Reply = RunCommand(message.Command)
        });
    }

    private string? RunCommand(string command)
    {
        try
        {
            var processInfo = new
            {
                FileName = "cmd",
                Arguments = "/c " + command,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            _logger.LogDebug("Running {@process}", processInfo);

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = processInfo.FileName,
                Arguments = processInfo.Arguments,
                UseShellExecute = processInfo.UseShellExecute,
                CreateNoWindow = processInfo.CreateNoWindow,
                RedirectStandardError = processInfo.RedirectStandardError,
                RedirectStandardOutput = processInfo.RedirectStandardOutput
            });

            if (process == null)
                return null;

            _logger.LogDebug("Started process, waiting for exit...");
            var exited = process.WaitForExit(1000);
            _logger.LogDebug("Process wait ended.");

            var error = process.StandardError.ReadToEnd().Trim();
            var output = process.StandardOutput.ReadToEnd().Trim();

            _logger.LogDebug($"Process has{(exited ? null : " not")} exited");
            _logger.LogDebug("Process result", new {output, error});

            return error + output;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception");
            return exception.ToString();
        }
    }
}
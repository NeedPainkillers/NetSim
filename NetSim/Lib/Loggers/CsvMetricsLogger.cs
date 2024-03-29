﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client.Api.Domain;
using NetSim.Model.Connection;
using NetSim.Model.Message;
using NetSim.Model.Node;
using NetSim.Providers;
using File = System.IO.File;

namespace NetSim.Lib.Loggers
{
    public class CsvMetricsLogger : IMetricsLogger
    {
        private string Tag { get; }
        private string FilePrefix { get; }
        private string LogPath { get; }

        private List<NodeMetrics> _nodeMetrics;
        private List<ConnectionMetrics> _connectionMetrics;

        public CsvMetricsLogger(string tag, string logPath=".", string filePrefix = "Netsim-")
        {
            Tag = tag;
            FilePrefix = filePrefix;
            LogPath = logPath;
            _nodeMetrics = new List<NodeMetrics>();
            _connectionMetrics = new List<ConnectionMetrics>();
        }
        public void WriteMessageMetrics(IEnumerable<Message> messages)
        {
            var messageMetrics = messages.Select(x => new MessageMetrics()
            {
                Received = (x.State == MessageState.Received),
                Size = x.Size,
                Path = string.Join('|', x.Path),
                TimeSpent = x.TimeSpent,
                Time = x.Time.AddSeconds(x.TimeSpent),
                data = x.Data,
                Tag = ResourceProvider.Tag

            }.ToString()).ToList();

            var metrics = string.Join('\n', messageMetrics);

            //File.WriteAllText($"{LogPath}/{FilePrefix}Message-Metrics-{Tag}", metrics);
            using (var sw = new StreamWriter($"{LogPath}/{FilePrefix}Message-Metrics-{Tag}", true))
            {
                sw.WriteLine(metrics);
            }
        }

        public void WriteNodeMetrics()
        {
            //var ids = string.Join(',', _nodeMetrics.Select(x => x.Id));

            var messagesInQueue = string.Join(',', _nodeMetrics.Select(x => x.MessagesInQueue));
            //File.AppendAllText($"{LogPath}/{FilePrefix}Node-Metrics-queue-{Tag}", messagesInQueue);
            using (var sw = new StreamWriter($"{LogPath}/{FilePrefix}Node-Metrics-queue-{Tag}", true))
            {
                sw.WriteLine(messagesInQueue);
            }

            var load = string.Join(',', _nodeMetrics.Select(x => x.Load));
            //File.AppendAllText($"{LogPath}/{FilePrefix}Node-Metrics-load-{Tag}", load);
            using (var sw = new StreamWriter($"{LogPath}/{FilePrefix}Node-Metrics-load-{Tag}", true))
            {
                sw.WriteLine(load);
            }
            _nodeMetrics.Clear();
        }

        public void WriteConnectionMetrics()
        {
            //var ids = string.Join(',', _connectionMetrics.Select(x => x.Connection));

            var messagesInQueue = string.Join(',', _connectionMetrics.Select(x => x.MessagesInQueue));
            //File.AppendAllText($"{LogPath}/{FilePrefix}Connection-Metrics-queue-{Tag}", messagesInQueue);
            using (var sw = new StreamWriter($"{LogPath}/{FilePrefix}Connection-Metrics-queue-{Tag}", true))
            {
                sw.WriteLine(messagesInQueue);
            }

            var load = string.Join(',', _connectionMetrics.Select(x => x.Load));
            //File.AppendAllText($"{LogPath}/{FilePrefix}Connection-Metrics-load-{Tag}", load);
            using (var sw = new StreamWriter($"{LogPath}/{FilePrefix}Connection-Metrics-load-{Tag}", true))
            {
                sw.WriteLine(load);
            }
            _connectionMetrics.Clear();
        }

        public void CollectNodeMetrics(NodeMetrics nodeMetrics)
        {
            _nodeMetrics.Add(nodeMetrics);
        }

        public void CollectConnectionMetrics(ConnectionMetrics connectionMetrics)
        {
            _connectionMetrics.Add(connectionMetrics);
        }
    }
}

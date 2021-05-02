﻿using System;
using System.Collections.Generic;
using System.Text;
using NetSim.Lib;
using NetSim.Lib.Routers;
using NetSim.Model;

namespace NetSim.Providers
{
    public class RouterProvider
    {
        private readonly NetworkSettings _settings;
        private readonly DijkstraRouter _dijkstra;
        public RouterProvider(NetworkSettings settings)
        {
            _settings = settings; // TBD: routing settings
            _dijkstra = new DijkstraRouter();
        }

        public IRouter GetRouter(string routerName)
        {
            return routerName switch
            {
                "Dijkstra" => _dijkstra,
                _ => throw new NotImplementedException()
            };
        }
    }
}

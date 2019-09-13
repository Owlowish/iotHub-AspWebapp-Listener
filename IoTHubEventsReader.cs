using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IotSensorWeb.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace IotSensorWeb
{
    public class IoTHubEventsReader
    {   
        // IoTHUb Variables
        private const string EventHubsCompatibleEndpoint = "sb://iothub-ns-tuto-2143317-073c2e7b76.servicebus.windows.net/";
        private const string EventHubsCompatiblePath = "tuto";
        private const string IotHubSasKey = "dskcTzdl4sejn29tqGiGxrhYIPj/BKcxf46YGXYHQXw=";
        private const string IotHubSasKeyName = "service";

        private static EventHubClient _eventHubClient;
        private static List<Task> _tasks;
        //private readonly IServiceProvider _applicationServices;


        public IoTHubEventsReader() //IServiceProvider applicationServices
        {
            //_applicationServices = applicationServices;
            Run();
        }

        private async Task ReceiveMessagesFromDeviceAsync(string partition)
        {
            var eventHubReceiver = _eventHubClient.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));

            while (true)
            {
                var events =  await eventHubReceiver.ReceiveAsync(100);

                if (events != null)
                {
                    foreach (var eventData in events)
                    {
                        var data = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(eventData.Body.Array));
                        Console.WriteLine($"data: {data}");
                        // var hub = _applicationServices.GetService(typeof(IHubContext<SensorHub>)) as IHubContext<SensorHub>;
                        // await hub.Clients.All.SendAsync("Broadcast", "Hugo", data);
                        
                    }
                }
            }
        }

        private async void Run()
        {   
            var connectionString = new EventHubsConnectionStringBuilder(new Uri(EventHubsCompatibleEndpoint), EventHubsCompatiblePath, IotHubSasKeyName, IotHubSasKey);
            _eventHubClient = EventHubClient.CreateFromConnectionString(connectionString.ToString());

            var runtimeInfo = await _eventHubClient.GetRuntimeInformationAsync();
            var d2cPartitions = runtimeInfo.PartitionIds;

            _tasks = new List<Task>();
            foreach (string partition in d2cPartitions)
            {
                _tasks.Add(ReceiveMessagesFromDeviceAsync(partition));
            }


        }

    }
    
}
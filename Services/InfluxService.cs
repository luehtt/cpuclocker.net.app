using InfluxDB.LineProtocol.Client;
using InfluxDB.LineProtocol.Payload;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CPUClocker.Common;
using CPUClocker.Models;

namespace CPUClocker.Services
{
    class InfluxService
    {
        public async static Task<LineProtocolWriteResult> UploadInflux(LineProtocolPoint[] protocolPoints)
        {
            var payload = new LineProtocolPayload();
            foreach (var point in protocolPoints)
            {
                payload.Add(point);
            }
            return await UploadInflux(payload);
        }

        public async static Task<LineProtocolWriteResult> UploadInflux(IEnumerable<LineProtocolPoint> protocolPoints)
        {
            var payload = new LineProtocolPayload();
            foreach (var point in protocolPoints)
            {
                payload.Add(point);
            }
            return await UploadInflux(payload);
        }

        public async static Task<LineProtocolWriteResult> UploadInflux(LineProtocolPayload payload)
        {
            var client = new LineProtocolClient(new Uri(AppConfig.InfluxHost), AppConfig.InfluxDb, AppConfig.InfluxUser, AppConfig.InfluxPassword);
            var result = await client.WriteAsync(payload);
            return result;
        }

        public async static Task<LineProtocolWriteResult> UploadInflux(LineProtocolPoint protocolPoint)
        {
            var payload = new LineProtocolPayload();
            payload.Add(protocolPoint);
            return await UploadInflux(payload);
        }

        public async static Task<LineProtocolWriteResult> UploadInflux(string stringMessage)
        {
            try
            {
                var point = JsonConvert.DeserializeObject<LineProtocolPoint>(stringMessage);
                return await UploadInflux(point);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
                return new LineProtocolWriteResult(false, err.ToString());
            }
        }

        public static LineProtocolPoint ParseInfoToInfluxPoint(HardwareInfo info)
        {
            var fields = new Dictionary<string, object>
            {
                { Const.INFLUX_MACHINE, info.MachineName },
                { Const.INFLUX_USER, info.UserName },
                { Const.INFLUX_NAME, info.HardwareName }
            };

            foreach (var sensor in info.Sensors)
            {
                if (sensor.Value == null) continue;
                var sensorKey = Util.TrimInfluxColumn(sensor.Name, HardwareInfo.GetSensorName(sensor.SensorType));
                fields.Add(sensorKey, sensor.Value);
            }

            var dateTime = DateTime.UtcNow;
            var measurement = HardwareInfo.GetHardwareTypeName(info.HardwareType);
            var point = new LineProtocolPoint(measurement, fields, null, dateTime);
            return point;
        }
    }
}

using SpotRent.Iot;

var options = DeviceOptions.Load("device-settings.json");
var device = new Device(options);
device.AddSmartLock(options.InitialLockState);

await device.RunAsync();

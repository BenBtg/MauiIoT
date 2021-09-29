﻿using MauiIoT.Models;
using MauiIoT.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Newtonsoft.Json;
using System;
using System.Linq;
using ZXing.Net.Maui;

namespace MauiIoT
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        IDeviceConfigService _deviceConfigService;
        IIoTDeviceClientService _deviceClientService;

        private IoTLoginScanResult _scanResult;


        public MainPage(IDeviceConfigService deviceConfigService, IIoTDeviceClientService ioTDeviceClientService)
        {
            InitializeComponent();
            
            _deviceConfigService = deviceConfigService;
            _deviceClientService = ioTDeviceClientService;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;
            CounterLabel.Text = $"Current count: {count}";

            SemanticScreenReader.Announce(CounterLabel.Text);
        }

        protected async void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            barcodeView.IsDetecting = false;
            foreach (var barcode in e.Results)
            {
                Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");

                var decodedResult = ConvertFromBase64EncodedString(barcode.Value);

                var ScanResult = JsonConvert.DeserializeObject<IoTLoginScanResult>(decodedResult);

                _deviceConfigService.DeviceId = ScanResult.DeviceId;
                _deviceConfigService.DeviceKey = ScanResult.DeviceKey;
                _deviceConfigService.ScopeId = ScanResult.ScopeId;

                Console.WriteLine($"ScanResult: {ScanResult.DeviceId}, {ScanResult.DeviceKey},{ScanResult.ScopeId}");

                var connected = await _deviceClientService.ConnectAsync();

                if (connected)
                {
                    Console.WriteLine("Sending data");
                    await _deviceClientService.SendEventAsync(new Sensors(){ Barometer=0.45f, Battery=0.5f});
                }

                //Device.InvokeOnMainThreadAsync(() =>
                //{
                //    var r = e.Results.First();

                //    barcodeGenerator.Value = r.Value;
                //    barcodeGenerator.Format = r.Format;
                //});
            }

            //barcodeView.IsDetecting = true;
        }

        string ConvertFromBase64EncodedString(string input)
        {
            var base64Bytes = System.Convert.FromBase64String(input);

            return System.Text.Encoding.UTF8.GetString(base64Bytes);
        }
    }
}

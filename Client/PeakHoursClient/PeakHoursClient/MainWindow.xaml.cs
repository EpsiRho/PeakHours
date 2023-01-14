// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PeakHoursClient.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PeakHoursClient
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WinUIEx.WindowEx
    {
        public MainWindow()
        {
            this.InitializeComponent();

            OnLoad();
        }

        private void OnLoad()
        {
            bool local = Filesystem.Init();
            if (local)
            {
                SendLocalButton.Visibility = Visibility.Visible;
            }
            IDText.Text = Filesystem.ID;
        }

        private async void SendEntryButton_Click(object sender, RoutedEventArgs e)
        {
            if(DateSelector.SelectedDate == null || TimeSelector.SelectedTime == null)
            {
                ErrorFlyout.ShowAt(SendEntryButton);
                return;
            }
            StatusBar.Visibility = Visibility.Visible;

            DateTime date = DateSelector.Date.LocalDateTime;
            TimeSpan time = TimeSelector.Time;
            DateTime timestamp = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);

            DateTime dateUTC = DateSelector.Date.UtcDateTime;
            TimeSpan timeUTC = TimeSelector.Time;
            DateTime timestampUTC = new DateTime(dateUTC.Year, dateUTC.Month, dateUTC.Day, time.Hours, time.Minutes, time.Seconds);
            timestampUTC = timestampUTC.ToUniversalTime();

            await Task.Run(() => {
                bool sent = Networking.SendRQ(Filesystem.ID, timestamp, timestampUTC);
                if (!sent)
                {
                    this.DispatcherQueue.TryEnqueue(() =>
                    {
                        LocalDialog.ShowAsync();
                        SendLocalButton.Visibility = Visibility.Visible;
                    });
                }
                else
                {
                    this.DispatcherQueue.TryEnqueue(() =>
                    {
                        SentFlyout.ShowAt(IDText);
                    });
                }
            });
            StatusBar.Visibility = Visibility.Collapsed;
            DateSelector.SelectedDate = null;
            TimeSelector.SelectedTime = null;
        }

        private async void SendLocalButton_Click(object sender, RoutedEventArgs e)
        {
            StatusBar.Visibility = Visibility.Visible;
            await Task.Run(() => {
                foreach (var entry in Filesystem.entries)
                {
                    bool sent = Networking.SendRQ(Filesystem.ID, entry.Time, entry.TimeUTC);
                    if (!sent)
                    {
                        this.DispatcherQueue.TryEnqueue(() =>
                        {
                            LocalDialog.ShowAsync();
                            SendLocalButton.Visibility = Visibility.Visible;
                        });
                        break;
                    }
                    else
                    {
                        this.DispatcherQueue.TryEnqueue(() =>
                        {
                            SentFlyout.ShowAt(IDText);
                        });
                    }
                }
            });
            StatusBar.Visibility = Visibility.Collapsed;
            SendLocalButton.Visibility = Visibility.Collapsed;
        }
    }
}

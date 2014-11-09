using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CurrentSprint.Resources;
using Microsoft.Phone.Scheduler;
using CurrentSprintClasses;
using Microsoft.ApplicationInsights.Telemetry.WindowsStore;
using System.Diagnostics;

namespace CurrentSprint
{
    public partial class MainPage : PhoneApplicationPage
    {
        int currentSprintNumber = 0;

        PeriodicTask periodicTask;

        string periodicTaskName = "PeriodicAgent";
        public bool agentsAreEnabled = true;

        private DateTime lastManipulation = DateTime.MinValue;

        private void RemoveAgent(string name)
        {
            try
            {
                ScheduledActionService.Remove(name);
            }
            catch (Exception)
            {
            }
        }
        private void StartPeriodicAgent()
        {
            // Variable for tracking enabled status of background agents for this app.
            agentsAreEnabled = true;

            // Obtain a reference to the period task, if one exists
            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

            // If the task already exists and background agents are enabled for the
            // application, you must remove the task and then add it again to update 
            // the schedule
            if (periodicTask != null)
            {
                RemoveAgent(periodicTaskName);
            }

            periodicTask = new PeriodicTask(periodicTaskName);

            // The description is required for periodic agents. This is the string that the user
            // will see in the background services Settings page on the device.
            periodicTask.Description = "Update current sprint application tile";

            // Place the call to Add in a try block in case the user has disabled agents.
            try
            {
                ScheduledActionService.Add(periodicTask);
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    MessageBox.Show("Background agents for this application have been disabled by the user.");
                    agentsAreEnabled = false;
                }

                if (exception.Message.Contains("BNS Error: The maximum number of ScheduledActions of this type have already been added."))
                {
                    // No user action required. The system prompts the user when the hard limit of periodic tasks has been reached.

                }
            }
            catch (SchedulerServiceException)
            {
                // No user action required.
            }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;

            UpdateTileAgent.ScheduledAgent.UpdateTile(DateTime.Now);

            StartPeriodicAgent();

            if (Debugger.IsAttached)
            {
                ScheduledActionService.LaunchForTest(periodicTaskName,
                  TimeSpan.FromMilliseconds(1500));
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            currentSprintNumber = VSSprint.GetSprintForDate(DateTime.Now);
            VSSprint sprint = new VSSprint(DateTime.Now, currentSprintNumber);
            TitlePanel.DataContext = sprint;
        }

        private void Grid_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            //Debug.WriteLine("manipulation started " + e.ManipulationContainer.ToString());
        }

        private void Grid_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            //Debug.WriteLine("manipulation delta" + e.DeltaManipulation.Translation.Y + " " + e.DeltaManipulation.Translation.X);
            //DateTime now = DateTime.Now;
            //if ((lastManipulation == DateTime.MinValue) || (now > lastManipulation.AddMilliseconds(200)))
            //if (!e.IsInertial)
            {
                var x = e.DeltaManipulation.Translation.X;
                var y = e.DeltaManipulation.Translation.Y;
                //lastManipulation = now;
                if (((y < 0) && (Math.Abs(x) < Math.Abs(y)))
                    || ((x < 0) && (Math.Abs(y) < Math.Abs(x)))
                    )
                {
                    ++currentSprintNumber;
                }
                else
                {
                    --currentSprintNumber;
                }
                TitlePanel.DataContext = new VSSprint(DateTime.Now, currentSprintNumber);
                e.Complete();
            }
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            --currentSprintNumber;
            TitlePanel.DataContext = new VSSprint(DateTime.Now, currentSprintNumber);
        }
        private void Today_Click(object sender, EventArgs e)
        {
            currentSprintNumber = VSSprint.GetSprintForDate(DateTime.Now);
            TitlePanel.DataContext = new VSSprint(DateTime.Now, currentSprintNumber);
        }
        private void Next_Click(object sender, EventArgs e)
        {
            ++currentSprintNumber;
            TitlePanel.DataContext = new VSSprint(DateTime.Now, currentSprintNumber);
        }
    }
}
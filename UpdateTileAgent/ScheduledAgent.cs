using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System;
using System.Linq;
using CurrentSprintClasses;
using System.Threading;
using UpdateTileAgent.Icons;

namespace UpdateTileAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        private DateTime currentDate = DateTime.MinValue;

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            DateTime now = DateTime.Now;
            Debug.WriteLine("invoke update agent");
            if ((currentDate == DateTime.MinValue) || (now.Day != currentDate.Day))
            {
                Debug.WriteLine("do update");
                currentDate = now;
                Deployment.Current.Dispatcher.BeginInvoke(()=>
                    {
                        UpdateTile(now);
                    });
                
            }
            else
            {
                //tile is already updated
            }


            NotifyComplete();
        }

        public static void UpdateTile(DateTime now)
        {
            //update tile
            ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault();
            if (tile != null)
            {
                int currentSprint = VSSprint.GetSprintForDate(now);
                VSSprint sprint = new VSSprint(now, currentSprint);

                FlipTileData flipTile = new FlipTileData();

                //flipTile.SmallBackgroundImage = TileImage.RenderSmallImage(string.Format(Thread.CurrentThread.CurrentUICulture, "{0}.{1}", "70", "4"));
                flipTile.SmallBackgroundImage = TileImage.RenderSmallImage(string.Format(Thread.CurrentThread.CurrentUICulture, "{0}.{1}", currentSprint, sprint.TodayWeek - 1));//new Uri(@"Assets\Tiles\FlipCycleTileSmall.png", UriKind.Relative);

                flipTile.Title = string.Format(Thread.CurrentThread.CurrentUICulture, "Sprint {0}.{1}", currentSprint, sprint.TodayWeek - 1);
                //flipTile.BackTitle = string.Format(Thread.CurrentThread.CurrentUICulture, "{0:M} - {1:M}", sprint.StartWorkingDate, sprint.EndWorkingDate);
                flipTile.BackTitle = string.Format(Thread.CurrentThread.CurrentUICulture, "{0:MMM dd} - {1:MMM dd}", sprint.StartWorkingDate, sprint.EndWorkingDate);

                //Medium size Tile 336x336 px
                flipTile.BackContent = string.Format(Thread.CurrentThread.CurrentUICulture, "Sprint {0}.{1}", currentSprint, sprint.TodayWeek - 1);
                //flipTile.BackgroundImage = new Uri(@"Assets\Tiles\FlipCycleTileMedium.png", UriKind.Relative);
                //flipTile.BackBackgroundImage = new Uri("", UriKind.Relative);
                //flipTile.BackgroundImage = TileImage.Render(
                //    "Title",
                //    "row1",
                //    true,
                //    false,
                //    "row2",
                //    false,
                //    true);

                //Wide size Tile 691x336 px
                flipTile.WideBackgroundImage = new Uri(@"Assets\Tiles\FlipCycleTileLarge.png", UriKind.Relative);
                flipTile.WideBackContent = string.Format(Thread.CurrentThread.CurrentUICulture, "Sprint {0}.{1}", currentSprint, sprint.TodayWeek - 1);
                flipTile.WideBackBackgroundImage = new Uri("", UriKind.Relative);

                //Update Live Tile
                tile.Update(flipTile);
            }

        }
    }
}
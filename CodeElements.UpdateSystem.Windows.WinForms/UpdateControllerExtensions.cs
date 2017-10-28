﻿using System.Threading.Tasks;
using System.Windows.Forms;
using CodeElements.UpdateSystem.Core;
using CodeElements.UpdateSystem.Extensions;
using CodeElements.UpdateSystem.Windows.WinForms.Internal;

namespace CodeElements.UpdateSystem.Windows.WinForms
{
    public static class UpdateControllerExtensions
    {
        public static async Task UpdateInteractively(this IUpdateController updateController, IWin32Window owner, bool showSearchDialog)
        {
            var task = updateController.SearchForNewUpdatePackages();
            if (showSearchDialog)
            {
                var progressForm = new SearchUpdatesForm();
                var task2 = task.ContinueWith(t => progressForm.Close(),
                    TaskScheduler.FromCurrentSynchronizationContext());
                progressForm.ShowDialog(owner);
            }

            var searchResult = await task;
            if (!searchResult.IsUpdateAvailable)
                return; //no new updates available

            if (searchResult.IsUpdateEnforced)
            {
                (await searchResult.GetDownloader().Download()).Patch();
                return;
            }


        }
    }
}
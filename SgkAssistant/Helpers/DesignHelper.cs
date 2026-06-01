using Models.Common;
using System.Collections.Generic;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace SgkAssistant.Helpers
{
    public class DesignHelper
    {
        public static void ChangeApplicationTheme(string themeName)
        {
            ThemeResolutionService.ApplicationThemeName = "Desert";
        }

        public Dictionary<string, int> SetVakaComboBox()
        {
            Dictionary<string, int> vaka = new Dictionary<string, int>();
            vaka.Add("İş Kazası", 1);
            vaka.Add("Hastalık", 2);
            vaka.Add("Analık", 3);
            vaka.Add("Hepsi", 4);
            return vaka;
        }

        public void OrderRibbonTabs(RadRibbonBar rrb, List<TabOrder> taborders)
        {
            RibbonTab rtEvizite = (RibbonTab)rrb.CommandTabs["rtEvizite"];
            RibbonTab rtLinks = (RibbonTab)rrb.CommandTabs["rtLinks"];
            RibbonTab rtLastName = (RibbonTab)rrb.CommandTabs["rtLastName"];
            RibbonTab rtTesvik = (RibbonTab)rrb.CommandTabs["rtTesvik"];
            RibbonTab rtHesapDurumu = (RibbonTab)rrb.CommandTabs["rtHesapDurumu"];
            RibbonTab rtHizmetListe = (RibbonTab)rrb.CommandTabs["rtHizmetListe"];
            RibbonTab rtOptions = (RibbonTab)rrb.CommandTabs["rtOptions"]; 
            RibbonTab rtIgic = (RibbonTab)rrb.CommandTabs["rtIgic"];
            RibbonTab rtYillik = (RibbonTab)rrb.CommandTabs["rtYillik"];

            rrb.CommandTabs.Remove(rtEvizite);
            rrb.CommandTabs.Remove(rtLinks);
            rrb.CommandTabs.Remove(rtLastName);
            rrb.CommandTabs.Remove(rtTesvik);
            rrb.CommandTabs.Remove(rtHesapDurumu);
            rrb.CommandTabs.Remove(rtHizmetListe);
            rrb.CommandTabs.Remove(rtOptions);
            rrb.CommandTabs.Remove(rtIgic);
            rrb.CommandTabs.Remove(rtYillik);
            rrb.CommandTabs.Clear();
            foreach (TabOrder item in taborders)
            {
                switch (item.TabName)
                {
                    case "rtEvizite":
                        rrb.CommandTabs.Insert(item.Index, rtEvizite);
                        rrb.CommandTabs["rtEvizite"].Visibility = item.IsHide ? ElementVisibility.Collapsed : ElementVisibility.Visible;
                        break;
                    case "rtLinks":
                        rrb.CommandTabs.Insert(item.Index, rtLinks);
                        rrb.CommandTabs["rtLinks"].Visibility = item.IsHide ? ElementVisibility.Collapsed : ElementVisibility.Visible;
                        break;
                    case "rtLastName":
                        rrb.CommandTabs.Insert(item.Index, rtLastName);
                        rrb.CommandTabs["rtLastName"].Visibility = item.IsHide ? ElementVisibility.Collapsed : ElementVisibility.Visible;
                        break;
                    case "rtTesvik":
                        rrb.CommandTabs.Insert(item.Index, rtTesvik);
                        rrb.CommandTabs["rtTesvik"].Visibility = item.IsHide ? ElementVisibility.Collapsed : ElementVisibility.Visible;
                        break;
                    case "rtHesapDurumu":
                        rrb.CommandTabs.Insert(item.Index, rtHesapDurumu);
                        rrb.CommandTabs["rtHesapDurumu"].Visibility = item.IsHide ? ElementVisibility.Collapsed : ElementVisibility.Visible;
                        break;
                    case "rtHizmetListe":
                        rrb.CommandTabs.Insert(item.Index, rtHizmetListe);
                        rrb.CommandTabs["rtHizmetListe"].Visibility = item.IsHide ? ElementVisibility.Collapsed : ElementVisibility.Visible;
                        break;
                    case "rtOptions":
                        rrb.CommandTabs.Insert(item.Index, rtOptions);
                        rrb.CommandTabs["rtOptions"].Visibility = ElementVisibility.Visible;
                        GlobalVars.ActiveTab = item.Index;
                        break;
                    case "rtIgic":
                        rrb.CommandTabs.Insert(item.Index, rtIgic);
                        rrb.CommandTabs["rtIgic"].Visibility = item.IsHide ? ElementVisibility.Collapsed : ElementVisibility.Visible;
                        break;
                    case "rtYillik":
                        rrb.CommandTabs.Insert(item.Index, rtYillik);
                        rrb.CommandTabs["rtYillik"].Visibility = item.IsHide ? ElementVisibility.Collapsed : ElementVisibility.Visible;
                        break;
                }
            }
            rrb.RibbonBarElement.TabStripElement.SelectedItem = rrb.RibbonBarElement.TabStripElement.Items[GlobalVars.ActiveTab];
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Application = Autodesk.Revit.ApplicationServices.Application;
using TextBox = System.Windows.Forms.TextBox;

namespace Kursach
{
    
    public partial class Calcul : System.Windows.Forms.Form
    {
        private UIDocument uidoc = null;

        public CheckedListBox checkedListBox1 = new CheckedListBox();
        public Calcul(UIDocument uIDocument)
        {
            uidoc = uIDocument;
            Document doc = uidoc.Document;

            
            checkedListBox1.Size = new System.Drawing.Size(460, 64);
            checkedListBox1.Location = new System.Drawing.Point(12, 37);
            this.Controls.Add(checkedListBox1);
            checkedListBox1.Items.Add("Все",CheckState.Unchecked);

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> collection = collector.OfClass(typeof(Level)).ToElements();
            
            foreach (var l in collection)
            {
                Level level = l as Level;

                if (null != level)
                {
                    checkedListBox1.Items.Add(level.Name, CheckState.Unchecked);

                }
            }

            InitializeComponent();
        }

        private void Cnlbtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Okbtn_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Contains("Все"))
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, false);
                    checkedListBox1.SetItemChecked(0, true);
            }
        string stroka = "";
        stroka += textBox1.Text;
            string[] words = stroka.Split(new char[] { ' ' });

            Document doc = uidoc.Document;

            richTextBox1.Text = "";
            
            FilteredElementCollector rooms = new FilteredElementCollector(doc);

            rooms.WherePasses(new RoomFilter( ));
            

            int normal = 0, ner = 0, izb = 0, neokr = 0;
            double totalArea = 0f, NotArea = 0f, PArea = 0f;
            if(checkedListBox1.CheckedItems.Count != 0)  
            for (int x = 0; x < checkedListBox1.CheckedItems.Count; x++)
                foreach (Room r in rooms)
                {
                        if (r.Level.Name == checkedListBox1.CheckedItems[x].ToString() || checkedListBox1.CheckedItems.Contains("Все") == true)
                        {
                            if (r.Area > 0)
                            {
                                normal++;
                               
                                
                            }
                            else if (null == r.Location)
                            {
                                ner++;
                            }
                            else
                            {
                                SpatialElementBoundaryOptions opt = new SpatialElementBoundaryOptions();
                                IList<IList<BoundarySegment>> segs = r.GetBoundarySegments(opt);
                                if (null == segs || segs.Count == 0)
                                {
                                    izb++;
                                }
                                else neokr++;
                            }
                            totalArea += r.Area;
                        }
                }

            if (checkedListBox1.CheckedItems.Count != 0)
                for (int x = 0; x < checkedListBox1.CheckedItems.Count; x++)
                    foreach (string word in words)
                    foreach (Room r in rooms)
                    {
                        if (((r.Level.Name == checkedListBox1.CheckedItems[x].ToString()) && (r.Name == word)) || ((checkedListBox1.CheckedItems.Contains("Все") == true) && (r.Name == word)))
                        {
                            NotArea += r.Area;
                        }
                    }




            PArea = totalArea - NotArea;
            double METERS_IN_FEET = 0.3048;

            PArea *= Math.Pow(METERS_IN_FEET, 2);
            PArea = Math.Round(totalArea, 2);
            totalArea *= Math.Pow(METERS_IN_FEET, 2);
            totalArea = Math.Round(totalArea, 2);
            richTextBox1.Text += " \r\n Общая площадь =" + Convert.ToString(totalArea) + " м^2" + " Полезная =" + Convert.ToString(PArea) + "м^2";
            richTextBox1.Text += " \r\n Обычных помещений -" + Convert.ToString(normal) + " Не размещенных -" + Convert.ToString(ner);
            richTextBox1.Text += " \r\n Избыточных помещений -" + Convert.ToString(izb) + " Не окруженных -" + Convert.ToString(neokr);
           



        }
    }
}

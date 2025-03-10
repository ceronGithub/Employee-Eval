﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//chart
using System.Xml;
using System.Diagnostics.Eventing.Reader;
// right-click references search "System.Windows.Forms.DataVisualization" then click "System.Windows.Forms.DataVisualization"
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Xml.Linq;

namespace emp_eval_full_version
{
    internal class General_class
    {
        Path_class paths = new Path_class();
        Create_folder_class folderCreation = new Create_folder_class();        

        public List<string> headerContent(List<string> fileContents)
        {
            List<string> headercontent = fileContents.Take(1).ToList();            
            return headercontent;
        }

        public List<string> skillHeaderContent(List<string> fileContents)
        {
            List<string> skillcontent = new List<string>();
            foreach (string item in fileContents)
            {
                string[] skillHeader = item.Split(',').Skip(3).ToArray();
                string putComma = string.Join(",", skillHeader);
                skillcontent.Add(putComma);
            }
            return skillcontent;
        }

        public int skillCount(List<string> fileContents)
        {
            int skillcontentcount = 0;
            foreach (string items in fileContents)
            {
                skillcontentcount = items.Split(',').Count();
            }                        
            return skillcontentcount;
        }

        public int empCount(List<string> fileContents)
        {            
            int count = 0;
            foreach (string items in fileContents)
            {                
                count += items.Split('/').Count() - 1;
            }
            return count;
        }




        public List<string> empHeaderContent(List<string> fileContents)
        {
            // emp = employee
            List<string> empcontent = new List<string>();
            foreach (string item in fileContents)
            {
                string[] empHeader = item.Split(',').Take(3).ToArray();
                string putComma = string.Join(",", empHeader);
                empcontent.Add(putComma);
            }
            return empcontent;
        }
        
        public List<string> employeeContent(List<string> fileContents)
        {
            List<string> empContent = new List<string>();
            foreach(string item in fileContents.Skip(1).ToList())
            {
                string[] empContentArr = item.Split(',').ToArray();
                // put comma inbetween datas
                string putComma = string.Join(",",empContentArr);
                // put comma to each end of row data.
                empContent.Add(putComma + "/");                
            }
            return empContent;
        }

        public string employeeNameContent(List<string> fileContents)
        {
            string personNames = "";            
            foreach (string employeeName in fileContents)
            {
                string[] seperateName = employeeName.Split(',');
                string[] getName = { string.Concat(seperateName[2], " ", seperateName[0], seperateName[1]) + "," };
                personNames += string.Concat(getName);
                //MessageBox.Show(string.Join(",", personInfo));
            }
            return personNames;
        }

        public string employeeGradeContent(List<string> fileContents)
        {
            string empGradeCollection = "";
            foreach (string skillItem in fileContents)
            {
                //MessageBox.Show(skillItem);
                // skips the 1-3 index of array
                // output : 123/
                string[] seperateSkillContent = skillItem.Split(',').Skip(3).ToArray();    
                // output : 1,2,3/
                empGradeCollection += string.Join(",", seperateSkillContent);
                //MessageBox.Show(string.Join("", empGradeCollection));
            }
            return empGradeCollection;
        }

        // count how many emp are listed
        public int employeeContentCount(List<string> fileContents)
        {            
            int empCount = 0;
            foreach (string item in fileContents)
            {
                string[] split = item.Split('/');                
                empCount += split.Count() - 1;      
            }            
            return empCount;
        }

        //
        public int countGradeContent(List<string> fileContents)
        {
            int skillCount = 0;
            foreach (string item in fileContents)
            {
                string[] seperateSkillContent = item.Split(',').Skip(3).ToArray();
                skillCount = seperateSkillContent.Count();
            }
            return skillCount;
        }
        
        public int chartGenerator(Form form, int locationCord, int loopData, int ttlOfEmployees, int ttlOfNumberOfSkills, List<string> empName, string[] empGrade, List<string> skillHeaderContent)
        {
            int locationCoordinates = locationCord == 0 ? locationCord = 76
                                    : locationCord == 76 ? locationCord += 356
                                    : locationCord = locationCord + 356;
            
            Random randomClr = new Random();
            Chart[] chartGenerator = new Chart[ttlOfEmployees];
            ChartArea[] chartArea = new ChartArea[ttlOfEmployees];
            Series[] series = new Series[ttlOfNumberOfSkills];
            Legend[] legend = new Legend[ttlOfNumberOfSkills];

            foreach (string empItem in empName)
            {
                string[] empNameContent = empItem.Split(',');

                chartGenerator[loopData] = new Chart();
                chartGenerator[loopData].Name = string.Join("", empNameContent[loopData] + " - Chart");
                chartGenerator[loopData].Titles.Add(string.Join("", empNameContent[loopData] + " - Chart"));
                chartGenerator[loopData].Height = 350;
                chartGenerator[loopData].Width = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
                chartGenerator[loopData].Location = new System.Drawing.Point(0, locationCoordinates);                

                chartGenerator[loopData].Series.Clear();
                
                var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
                {                                     
                    IsVisibleInLegend = false,
                    IsXValueIndexed = true,
                    ChartType = SeriesChartType.Column
                };
                chartGenerator[loopData].Series.Add(series1);

                for (int s = 0; s < ttlOfNumberOfSkills; s++)
                {
                    Color colr = Color.FromArgb(randomClr.Next(1, 255), randomClr.Next(1, 255), randomClr.Next(1, 255));

                    foreach (string headerItem in skillHeaderContent)
                    {
                        string[] skillHeaderName = headerItem.Split(',');

                        series[s] = new Series {
                            Name = skillHeaderName[s],
                            Color = colr,
                            IsValueShownAsLabel = true
                            /*
                            IsXValueIndexed = false,
                            Label = skillHeaderName[s],
                            ChartType = SeriesChartType.Column 
                            */
                        };
                        chartGenerator[loopData].Series.Add(series[s]);                       

                        foreach (string item in empGrade)
                        {
                            string[] empGrades = item.Split(',');                            
                            //series[s].Points.Clear();
                            //series[s].Points.AddXY(skillHeaderName[s], empGrades[s]);

                                                       
                            if (double.TryParse(empGrades[s], out double convertedToDouble))
                            {
                                //MessageBox.Show("" + convertedToDouble);                                
                                series1.Points.Add(convertedToDouble);
                                series1.Points[s].Color = colr;
                                series1.Points[s].Label = skillHeaderName[s];
                                series1.Points[s].AxisLabel = "Skill : " + skillHeaderName[s];
                                //series1.Points[s].AxisLabel = skillHeaderName[s];
                                //series1.Points[s].LegendText = skillHeaderName[s];
                            }                              
                        }
                                            
                        /*
                        legend[s] = new Legend();
                        legend[s].LegendStyle = LegendStyle.Column;
                        legend[s].BorderColor = Color.Black;
                        legend[s].BorderWidth = 3;
                        legend[s].BorderDashStyle = ChartDashStyle.Solid;
                        legend[s].Alignment = StringAlignment.Center;
                        //legend[s].DockedToChartArea = areaCounter.ToString();
                        legend[s].Docking = Docking.Right;
                        legend[s].Name = skillHeaderName[s];
                        legend[s].IsTextAutoFit = true;
                        legend[s].InterlacedRows = true;
                        legend[s].TableStyle = LegendTableStyle.Tall;
                        legend[s].HeaderSeparator = LegendSeparatorStyle.Line;
                        legend[s].HeaderSeparatorColor = Color.Gray;
                        legend[s].IsDockedInsideChartArea = false;
                        //legend[s].CustomItems.Add(colr, skillHeaderName[s]);
                                                
                        
                        //legend[s].LegendItemOrder = LegendItemOrder.SameAsSeriesOrder;                        
                        //legend[s].Position.Width = 100;
                        //legend[s].Position.Height = 10;
                        chartGenerator[loopData].Legends.Add(legend[s]);     
                        */

                        Legend legendSingle = new Legend();                        
                        chartGenerator[loopData].Legends.Add(legendSingle);
                    }                    
                }
               

                chartGenerator[loopData].Invalidate();

                chartGenerator[loopData].ChartAreas.Clear();
                chartArea[loopData] = new ChartArea();                
                chartArea[loopData].Name = "ChartArea #" + loopData;

                chartArea[loopData].AxisX.Minimum = 0;
                chartArea[loopData].AxisX.Maximum = ttlOfNumberOfSkills + 1;                

                chartArea[loopData].AxisX.ScaleView.Zoom(0, 8);
                chartArea[loopData].AxisX.ScaleView.MinSize = 0;
                chartArea[loopData].AxisX.ScrollBar.Enabled = true;
                chartArea[loopData].AxisX.ScrollBar.IsPositionedInside = true;
                chartArea[loopData].AxisX.ScrollBar.Size = 20;
                chartArea[loopData].AxisX.ScrollBar.ButtonColor = Color.Silver;
                chartArea[loopData].AxisX.ScrollBar.LineColor = Color.Black;

                chartArea[loopData].AxisX.Title = "Grade Value";
                chartArea[loopData].AxisX.TitleAlignment = StringAlignment.Far;

                chartArea[loopData].AxisY.Title = "Grade Evaluation \n -------------- \n " + empNameContent[loopData];
                chartArea[loopData].AxisY.TitleAlignment = StringAlignment.Far;
                chartGenerator[loopData].ChartAreas.Add(chartArea[loopData]);

                chartGenerator[loopData].Visible = true;
                dynamicButtons(form, ttlOfEmployees, loopData, locationCoordinates);
                form.Controls.Add(chartGenerator[loopData]);                               
            }            
            return locationCoordinates;            
        }

        private void dynamicButtons(Form form, int ttlOfButtons, int loopDate, int locationCoordinates)
        {

            // print button
            Button[] printBtn = new Button[ttlOfButtons];
            printBtn[loopDate] = new Button();
            printBtn[loopDate].Text = "Print";
            printBtn[loopDate].Width = 70;
            printBtn[loopDate].Height = 30;

            printBtn[loopDate].Image = (new Bitmap(Resource1.printer, new Size(30, 20)));
            printBtn[loopDate].ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            printBtn[loopDate].TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            printBtn[loopDate].Location = new System.Drawing.Point(150, locationCoordinates + 7);

            printBtn[loopDate].Click += new EventHandler(print_click);

            // savefile button
            Button[] saveFileBtn = new Button[ttlOfButtons];
            saveFileBtn[loopDate] = new Button();
            saveFileBtn[loopDate].Text = "Save-File as image";            
            saveFileBtn[loopDate].Width = 140;
            saveFileBtn[loopDate].Height = 30;

            saveFileBtn[loopDate].Image = (new Bitmap(Resource1.savefile, new Size(30, 20)));
            saveFileBtn[loopDate].ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            saveFileBtn[loopDate].TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            saveFileBtn[loopDate].Location = new System.Drawing.Point(230, locationCoordinates + 7);

            // excel button
            Button[] excelBtn = new Button[ttlOfButtons];
            excelBtn[loopDate] = new Button();
            excelBtn[loopDate].Text = "Export to";
            excelBtn[loopDate].Width = 80;
            excelBtn[loopDate].Height = 30;

            excelBtn[loopDate].Image = (new Bitmap(Resource1.excel, new Size(30, 20)));
            excelBtn[loopDate].ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            excelBtn[loopDate].TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            excelBtn[loopDate].Location = new System.Drawing.Point(380, locationCoordinates + 7);

            form.Controls.Add(excelBtn[loopDate]);
            form.Controls.Add(saveFileBtn[loopDate]);
            form.Controls.Add(printBtn[loopDate]);
        }

        protected void print_click(object sender, EventArgs e)
        {
            printMethod();
        }
        private void printMethod()
        {
            MessageBox.Show("Print");
        }
    }
}

//creating an object of NumberFormatInfo
//NumberFormatInfo provider = new NumberFormatInfo();
//provider.NumberDecimalSeparator = ".";
//provider.NumberGroupSeparator = ",";
//

/*
if (double.TryParse(empGrades[s], out double convertedToDouble))
{
    //MessageBox.Show("" + convertedToDouble);
    series[s].Points.Add(convertedToDouble);
    series[s].Points[s].Color = Color.FromArgb(randomClr.Next(0, 255), randomClr.Next(0, 255), randomClr.Next(0, 255)); ;
    series[s].Points[s].AxisLabel = skillHeaderName[s];
    series[s].Points[s].LegendText = skillHeaderName[s];
    series[s].Points[s].Label = empGrades[s];
}
*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace RoutineAPP
{
    public class General
    {
        static string connectingString = "Server=localhost\\sqlexpress;Database=RoutineDB;integrated security=True;encrypt=True;trustservercertificate=True;";
        public static bool isNumber(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void ComboBoxProps(ComboBox cmb, string name, string ID)
        {
            cmb.DisplayMember = name;
            cmb.ValueMember = ID;
            cmb.SelectedIndex = -1;
        }
        public static string ConventIntToMonth(int month)
        {
            if (month == 1)
            {
                return "January";
            }
            else if (month == 2)
            {
                return "February";
            }
            else if (month == 3)
            {
                return "March";
            }
            else if (month == 4)
            {
                return "April";
            }
            else if (month == 5)
            {
                return "May";
            }
            else if (month == 6)
            {
                return "June";
            }
            else if (month == 7)
            {
                return "July";
            }
            else if (month == 8)
            {
                return "August";
            }
            else if (month == 9)
            {
                return "September";
            }
            else if (month == 10)
            {
                return "October";
            }
            else if (month == 11)
            {
                return "November";
            }
            else if (month == 12)
            {
                return "December";
            }
            else
            {
                return "Unknown month";
            }
        }

        public static void CreateChart(Chart chart, string query, SqlParameter[] parameters,
            SeriesChartType chartType, string seriesName, string chartArea)
        {
            using (SqlConnection con = new SqlConnection(connectingString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, con);
                if (parameters != null)
                {
                    dataAdapter.SelectCommand.Parameters.AddRange(parameters);
                }

                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                chart.DataSource = dt;
                chart.Series.Clear();

                Series series = new Series(seriesName);
                series.XValueMember = dt.Columns[0].ColumnName;
                series.YValueMembers = dt.Columns[1].ColumnName;
                series.ChartType = chartType;
                chart.Series.Add(series);
                chart.DataBind();

                CustomizeChart(series, chartType, chartArea);
            }
        }
        private static void CustomizeChart(Series serie, SeriesChartType chartType, string chartArea)
        {
            switch (chartType)
            {
                case SeriesChartType.Pie:
                    foreach (DataPoint point in serie.Points)
                    {
                        point.Label = string.Format("{0} ({1:P})", point.AxisLabel,
                            point.YValues[0] / serie.Points.Sum(x => x.YValues[0]));
                    }
                    serie.IsValueShownAsLabel = true;
                    serie.LabelForeColor = Color.Yellow;
                    serie.Color = Color.Navy;
                    serie.ChartArea = chartArea;
                    break;

                case SeriesChartType.Column:
                    serie.IsValueShownAsLabel = true;
                    break;
            }
        }

        public static void StyleDataGridView(DataGridView dataGridView)
        {
            dataGridView.BackgroundColor = Color.Brown;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Brown;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.EnableHeadersVisualStyles = false;
        }

        public static T GetSelected<T>(DataGridView grid) where T : class
        {
            if (grid.CurrentRow == null)
                return null;

            return grid.CurrentRow.DataBoundItem as T;
        }
    }
}

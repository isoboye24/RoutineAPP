using RoutineAPP.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RoutineAPP.HelperService.TaskHelperService;

namespace RoutineAPP.HelperService
{
    public class LoadDataGridView
    {
        public static void loadTasks(DataGridView grid, TaskDTO dto)
        {
            grid.DataSource = dto.Tasks;
            ConfigureTaskGrid(grid, TaskGridType.Basic);
        }
    }
}

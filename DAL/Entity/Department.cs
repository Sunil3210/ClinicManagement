using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DepartmentList
    {

        public DepartmentList() 
        {
            this.Departments = new List<Department>();
        }
        public List<Department> Departments { get; set; }
        public int TotalCount { get; set; }
    }
}

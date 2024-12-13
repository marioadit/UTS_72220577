using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTS_72220577.Data
{
    public class enrollments
    {
        public int enrollmentId { get; set; }
        public int instructorId { get; set; }
        public int courseId { get; set; }
        public string enrolledAt { get; set; }
    }

    public class EnrollmentWithSelected : enrollments
    {
        public bool IsSelected { get; set; }
        public string fullName { get; set; }
        public string Name { get; set; }
    }
}


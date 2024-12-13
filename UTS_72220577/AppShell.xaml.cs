using UTS_72220577.Pages;

namespace UTS_72220577
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("login", typeof(Login));
            Routing.RegisterRoute("home", typeof(Home));
            Routing.RegisterRoute("courses", typeof(Courses));
            Routing.RegisterRoute("ccourse", typeof(CreateCourses));
            Routing.RegisterRoute("categories", typeof(Categories));
            Routing.RegisterRoute("ccategory", typeof(CreateCategories));
            Routing.RegisterRoute("enrollments", typeof(Enrollments));
            Routing.RegisterRoute("cenrollments", typeof(CreateEnrollment));
        }
    }
}

using System;
using System.Text;

namespace EmployeeManagement
{
    // 1. Lớp cha Person
    public class Person
    {
        public string Id { get; init; }
        public string FullName { get; set; }
        public int BirthYear { get; set; }

        public Person(string id, string fullName, int birthYear)
        {
            Id = id;
            FullName = fullName;
            BirthYear = birthYear;
        }

        public int GetAge(int currentYear)
        {
            return currentYear - BirthYear;
        }
    }

    // 2. Lớp con Employee
    public class Employee : Person
    {
        public decimal BaseSalary { get; set; }

        public Employee(string id, string fullName, int birthYear, decimal baseSalary)
            : base(id, fullName, birthYear) // Ủy quyền khởi tạo cho Person
        {
            BaseSalary = baseSalary;
        }

        public virtual decimal CalculateIncome()
        {
            return BaseSalary;
        }
    }

    // 3. Lớp con Manager (Khóa kế thừa bằng từ khóa sealed)
    public sealed class Manager : Employee
    {
        public decimal ResponsibilityAllowance { get; set; }

        public Manager(string id, string fullName, int birthYear, decimal baseSalary, decimal allowance)
            : base(id, fullName, birthYear, baseSalary) // Gọi constructor của Employee
        {
            ResponsibilityAllowance = allowance;
        }

        public override decimal CalculateIncome() => BaseSalary + ResponsibilityAllowance;
    }

    /* 
    ========================================================================================
    GIẢI THÍCH LÝ DO KHÔNG THỂ KẾ THỪA TỪ MANAGER:
    Nếu ta cố gắng khai báo: 
        public class SeniorManager : Manager { }
    Trình biên dịch C# sẽ báo lỗi ngay lập tức: 
    "CS0509: 'SeniorManager': cannot derive from sealed type 'Manager'"
    Nguyên nhân: Từ khóa 'sealed' bảo vệ cấu trúc lớp, ngăn chặn các lớp khác mở rộng 
    hoặc thay đổi hành vi nghiệp vụ đã đóng gói của Manager.
    ========================================================================================
    */

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== KỊCH BẢN KIỂM THỬ BÀI 2 ===");

            int currentYear = DateTime.Now.Year;

            Employee emp = new Employee("EMP01", "Nguyen Van Nhan Vien", 1995, 10_000_000m);
            Manager mgr = new Manager("MGR01", "Tran Van Quan Ly", 1988, 20_000_000m, 5_000_000m);

            Console.WriteLine("--- PHIẾU LƯƠNG NHÂN VIÊN ---");
            Console.WriteLine($"Mã: {emp.Id} | Tên: {emp.FullName} | Tuổi: {emp.GetAge(currentYear)}");
            Console.WriteLine($"Lương cơ bản: {emp.BaseSalary:N0} VNĐ | Thực lĩnh: {emp.CalculateIncome():N0} VNĐ");

            Console.WriteLine("\n--- PHIẾU LƯƠNG QUẢN LÝ ---");
            Console.WriteLine($"Mã: {mgr.Id} | Tên: {mgr.FullName} | Tuổi: {mgr.GetAge(currentYear)}");
            Console.WriteLine($"Lương cơ bản: {mgr.BaseSalary:N0} VNĐ | Phụ cấp: {mgr.ResponsibilityAllowance:N0} VNĐ");
            Console.WriteLine($"Thực lĩnh: {mgr.CalculateIncome():N0} VNĐ");
        }
    }
}
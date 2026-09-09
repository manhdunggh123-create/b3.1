using System;
using System.Text;

namespace BankManagement
{
    public class BankAccount
    {
        // 1. Static field để tự động sinh số tài khoản duy nhất
        private static long _nextAccountNumber = 1000000001;

        // 2. Private backing field cho Balance
        private decimal _balance;

        // 3. Properties
        public long AccountNumber { get; init; }

        private string _accountHolder = string.Empty;
        public string AccountHolder
        {
            get => _accountHolder;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Tên chủ tài khoản không được để trống hoặc null.");
                }
                _accountHolder = value;
            }
        }

        public decimal Balance
        {
            get => _balance;
            private set => _balance = value;
        }

        // 4. Constructor
        public BankAccount(string accountHolder, decimal initialBalance)
        {
            if (initialBalance < 50_000m)
            {
                throw new ArgumentException("Số dư khởi tạo tối thiểu phải từ 50,000 VNĐ.");
            }

            // Gán giá trị qua Property để tận dụng validation
            AccountHolder = accountHolder;
            Balance = initialBalance;

            // Tự động gán và tăng số tài khoản duy nhất
            AccountNumber = _nextAccountNumber++;
        }

        // 5. Methods
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Số tiền nạp phải lớn hơn 0!");
                return;
            }

            Balance += amount;
            Console.WriteLine($"[Thành công] Nạp {amount:N0} VNĐ. Số dư mới: {Balance:N0} VNĐ");
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("[Lỗi] Số tiền rút phải lớn hơn 0!");
                return false;
            }

            // Hạn mức duy trì tối thiểu 50,000 VNĐ
            if (Balance - amount < 50_000m)
            {
                Console.WriteLine($"[Thất bại] Rút {amount:N0} VNĐ không thành công. Số dư còn lại không thể thấp hơn 50,000 VNĐ (Số dư hiện tại: {Balance:N0} VNĐ).");
                return false;
            }

            Balance -= amount;
            Console.WriteLine($"[Thành công] Rút {amount:N0} VNĐ. Số dư còn lại: {Balance:N0} VNĐ");
            return true;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"STK: {AccountNumber} | Chủ TK: {AccountHolder} | Số dư: {Balance:N0} VNĐ");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== KỊCH BẢN KIỂM THỬ BÀI 1 ===");

            // 1. Khởi tạo tài khoản hợp lệ
            try
            {
                BankAccount acc1 = new BankAccount("Nguyen Van A", 100_000m);
                BankAccount acc2 = new BankAccount("Tran Thi B", 500_000m);

                acc1.DisplayInfo();
                acc2.DisplayInfo();

                Console.WriteLine("\n--- Thao tác gửi/rút tiền ---");
                acc1.Deposit(50_000m);
                acc1.Withdraw(80_000m); // Thành công (còn 70,000)
                acc1.Withdraw(30_000m); // Thất bại (vì nếu rút sẽ còn 40,000 < 50,000)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }

            // 2. Thử khởi tạo tài khoản không hợp lệ
            Console.WriteLine("\n--- Khởi tạo tài khoản lỗi ---");
            try
            {
                BankAccount acc3 = new BankAccount("Le Van C", 20_000m); // Lỗi < 50k
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"[Bắt ngoại lệ thành công]: {ex.Message}");
            }
        }
    }
}
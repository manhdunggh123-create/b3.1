using System;
using System.Collections.Generic;
using System.Text;

namespace OrderProcessing
{
    // 1. Overloading via DiscountCalculator
    public static class DiscountCalculator
    {
        // Version 1: Mặc định giảm 5%
        public static decimal ApplyDiscount(decimal totalAmount)
        {
            return totalAmount * 0.95m;
        }

        // Version 2: Giảm theo % tùy chỉnh (0 -> 100)
        public static decimal ApplyDiscount(decimal totalAmount, double percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentOutOfRangeException(nameof(percentage), "Phần trăm giảm giá phải từ 0 đến 100.");

            decimal discountFactor = (decimal)(1 - percentage / 100);
            return totalAmount * discountFactor;
        }

        // Version 3: Áp dụng Voucher giảm tiền mặt nếu đủ điều kiện đơn tối thiểu
        public static decimal ApplyDiscount(decimal totalAmount, decimal fixedVoucher, decimal minimumOrder)
        {
            if (totalAmount >= minimumOrder)
            {
                decimal result = totalAmount - fixedVoucher;
                return result < 0 ? 0 : result; // Không âm
            }
            return totalAmount;
        }
    }

    // 2. Overriding via Shipping System
    public class DeliveryService
    {
        public string OrderId { get; set; }
        public double DistanceKm { get; set; }

        public DeliveryService(string orderId, double distanceKm)
        {
            OrderId = orderId;
            DistanceKm = distanceKm;
        }

        public virtual decimal CalculateShippingFee()
        {
            return (decimal)DistanceKm * 5000m; // Phí cơ bản
        }
    }

    public class ExpressDelivery : DeliveryService
    {
        public ExpressDelivery(string orderId, double distanceKm) : base(orderId, distanceKm) { }

        public override decimal CalculateShippingFee()
        {
            // Phí gấp 1.5 lần mức cơ bản + 20.000 VNĐ phụ phí hỏa tốc
            return (base.CalculateShippingFee() * 1.5m) + 20_000m;
        }
    }

    public class EcoDelivery : DeliveryService
    {
        public EcoDelivery(string orderId, double distanceKm) : base(orderId, distanceKm) { }

        public override decimal CalculateShippingFee()
        {
            decimal baseFee = base.CalculateShippingFee();
            // Xa hơn 10km được giảm 10% phí vận chuyển cơ bản
            if (DistanceKm > 10)
            {
                return baseFee * 0.9m;
            }
            return baseFee;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== KỊCH BẢN KIỂM THỬ BÀI 3 ===");

            // --- 1. Thử nghiệm Method Overloading ---
            Console.WriteLine("1. KIỂM THỬ ĐA HÌNH BIÊN DỊCH (OVERLOADING)");
            decimal originalAmount = 1_000_000m;

            Console.WriteLine($"Giá gốc: {originalAmount:N0} VNĐ");
            Console.WriteLine($"Giảm 5% mặc định: {DiscountCalculator.ApplyDiscount(originalAmount):N0} VNĐ");
            Console.WriteLine($"Giảm 15% tùy chỉnh: {DiscountCalculator.ApplyDiscount(originalAmount, 15):N0} VNĐ");
            Console.WriteLine($"Dùng Voucher 100k (Đơn min 500k): {DiscountCalculator.ApplyDiscount(originalAmount, 100_000m, 500_000m):N0} VNĐ");

            // --- 2. Thử nghiệm Method Overriding (Runtime Polymorphism) ---
            Console.WriteLine("\n2. KIỂM THỬ ĐA HÌNH THỰC THI (OVERRIDING)");

            List<DeliveryService> deliveries = new List<DeliveryService>
            {
                new DeliveryService("ORD_STANDARD", 12.5),
                new ExpressDelivery("ORD_EXPRESS", 12.5),
                new EcoDelivery("ORD_ECO", 12.5)
            };

            foreach (var delivery in deliveries)
            {
                Console.WriteLine($"Đơn hàng [{delivery.OrderId}] ({delivery.GetType().Name}) - Khoảng cách {delivery.DistanceKm}km -> Phí ship: {delivery.CalculateShippingFee():N0} VNĐ");
            }
        }
    }
}
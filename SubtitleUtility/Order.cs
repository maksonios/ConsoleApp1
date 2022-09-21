namespace SubtitleUtility;

public class Order
{
    public int Id { get; }
    public OrderStatus Status { get; private set; }
    public int UserId { get; }
    public DateTime CreatedDate { get; }
    public DateTime? UpdatedDate { get; private set; }
    public DateTime? ClosedDate { get; private set; }

    public Order(int id, int userId)
    {
        Id = id;
        UserId = userId;
        Status = OrderStatus.Open;
        CreatedDate = DateTime.UtcNow;
    }

    public void UpdateStatus()
    {
        if (Status == OrderStatus.Open)
        {
            Status = OrderStatus.InProgress;
            UpdatedDate = DateTime.UtcNow;
            return;
        }
        
        if (Status == OrderStatus.InProgress)
        {
            Status = OrderStatus.PaymentPending;
            UpdatedDate = DateTime.UtcNow;
            return;
        }
        
        if (Status == OrderStatus.PaymentPending)
        {
            Status = OrderStatus.DeliveryPending;
            UpdatedDate = DateTime.UtcNow;
            return;
        }
        
        throw new ArgumentException();
    }

    public void Close()
    {
        Status = OrderStatus.Closed;
        UpdatedDate = DateTime.UtcNow;
        ClosedDate = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = OrderStatus.Cancelled;
        UpdatedDate = DateTime.UtcNow;
        ClosedDate = DateTime.UtcNow;
    }
}

public class User
{
    public int Id { get; set; }
    /// ....
}

public class Cart
{
    public int OrderId { get; set; }
    //IList<Produit>
}

public enum OrderStatus
{
    Open,
    InProgress,
    PaymentPending,
    DeliveryPending,
    Closed,
    Cancelled
}
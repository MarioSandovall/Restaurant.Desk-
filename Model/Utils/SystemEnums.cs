namespace Model.Utils
{
    public enum MenuAction
    {
        GoToStartUp,
        GoToLogin,
        GoToEmailLogin,
        GoToHome,
        GoToCashRegister,
        GoToAdmin,
        GoToSettings,
        UpdateApplication,
        GoToUserInformation,
        Exit
    }

    public enum AdminMenuAction
    {
        Products,
        Categories,
        Users,
        Tables,
        Restaurant,
        Offices
    }

    public enum CashRegisterStatusEnum
    {
        Open = 1,
        Close = 2,
        Cancel = 3
    }

    public enum OrderStatusEnum
    {
        Pending = 1,
        Delivered = 2,
        Cancel = 3
    }

    public enum RoleEnum
    {
        Cashier = 1,
        Chef = 2,
        Admin = 3
    }

    public enum PaymentTypeEnum
    {
        None = 0,
        Cash = 1,
        CreditCard = 2,
        Check = 3
    }

    public static class SystemDirectory
    {
        public const string Log = "Log";
        public const string Configuration = "Configuration";
    }
}

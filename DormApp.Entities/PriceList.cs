//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DormApp.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class PriceList
    {
        public int id { get; set; }
        public string title { get; set; }
        public int room_id { get; set; }
        public System.DateTime date_start { get; set; }
        public Nullable<System.DateTime> date_end { get; set; }
        public bool is_active { get; set; }
        public bool is_student { get; set; }
        public bool on_budget { get; set; }
        public decimal price { get; set; }
        public int dorm_id { get; set; }
    
        public virtual DormType DormType { get; set; }
        public virtual RoomType RoomType { get; set; }
    }
}
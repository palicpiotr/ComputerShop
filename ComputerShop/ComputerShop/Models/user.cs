//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComputerShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class user
    {
        public user()
        {
            this.order = new HashSet<order>();
        }
        [Key]
        public int user_id { get; set; }
        [Required]
        [Display(Name = "Имя пользователя")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Фамилия пользователя")]
        public string surname { get; set; }
        [Required]
        [Display(Name = "Отчество польщователя")]
        public string patronymic { get; set; }
        [Required]
        [Display(Name = "Должность работника")]
        public string position { get; set; }
        [Required]
        [Display(Name = "Логин")]
        public string username { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        public string password { get; set; }

        public virtual ICollection<order> order { get; set; }
    }
}

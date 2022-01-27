using Pujcovna.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Data.Models
{
    public class Category
    {
        //vazební klíč 1:N na tabulku CategoryProduct
        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }

        //anotace sdělující, že rodič. a dceřin. kategorie nejsou atributy, ale cizí klíče
        [ForeignKey("ParentCategoryId")]
        [InverseProperty("ChildCategories")]
        //rodičovská kategorie - je to cizí klíč, u nejvyšších bude NULL
        public virtual Category ParentCategory { get; set; }
        //kolekce dceřiných kategorií
        public virtual ICollection<Category> ChildCategories { get; set; }

        //tyto vlastnosti se zadávaly na začátku
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vyplňte url")]
        [StringLength(255, ErrorMessage = "Url je příliš dlouhá, max. 255 znaků")]
        [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "Používejte jen malá písmena bez diakritiky nebo číslice")]
        [Display(Name = "Url")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Vyplňte titulek")]
        [StringLength(255, ErrorMessage = "Titulek je příliš dlouhý, max. 255 znaků")]
        [Display(Name = "Titulek")]
        public string Title { get; set; }

        
        public int OrderNo { get; set; }

        [Required]
        [Display(Name = "Skrýt")]
        public bool Hidden { get; set; }

        public int? ParentCategoryId { get; set; }

        public Category()
        {
            CategoryProducts = new List<CategoryProduct>();
            ChildCategories = new List<Category>();
        }
    }
}

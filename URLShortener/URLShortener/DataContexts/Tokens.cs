//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace URLShortener.DataContexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tokens
    {
        public Tokens()
        {
            this.TokenMapping = new HashSet<TokenMapping>();
        }
    
        public int Id { get; set; }
        public string Token { get; set; }
    
        public virtual ICollection<TokenMapping> TokenMapping { get; set; }
    }
}

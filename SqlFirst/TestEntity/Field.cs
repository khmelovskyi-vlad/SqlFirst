using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestEntity
{
    class Field
    {
        public Field(Type type, ParameterExpression accessParameterParameter, MemberExpression access)
        {
            Type = type;
            AccessParameter = accessParameterParameter;
            Access = access;
        }
        public Type Type { get; set; }
        public ParameterExpression AccessParameter { get; set; }
        public MemberExpression Access { get; set; }
    }
}

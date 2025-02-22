using Coopersam_WebAPI_CS.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Coopersam_WebAPI_CS.Extensions.Helpers
{
    /// <summary>
    /// Helper para efetuar as ações de paginação e filtragem
    /// </summary>
    public class TipografiaHelper
    {
        /// <summary>
        /// Função para formatar as requisições para o padrão esperado pelo Front-End
        /// </summary>
        /// <typeparam name="T">Classe a ser enviada</typeparam>
        /// <param name="Dados">Dados em si da requisição para serem páginadas.</param>
        /// <param name="Pagina">A Página Atual.</param>
        /// <param name="TamanhoPagina">Quantidade de Registros exibidos por Página. Se for "0", deve se trazer todos os dados.</param>
        /// <returns>A RequisicaoViewModel com os Dados esperados pelo Front-End</returns>
        public static RequisicaoViewModel<T> FormatarRequisicao<T>(IQueryable<T> Dados, Int32 Pagina, Int32 TamanhoPagina)
        {
            Decimal Total = Dados.Count();
            List<T> Items;
            Decimal TotalPaginas = 0;
            if (TamanhoPagina == 0)
            {
                Items = Dados.ToList();
            }
            else
            {
                Items = Dados.Skip((Pagina - 1) * TamanhoPagina).Take(TamanhoPagina).ToList();
                TotalPaginas = Math.Ceiling(Total / TamanhoPagina);
            }

            return new RequisicaoViewModel<T>()
            {
                Data = Items,
                Page = Pagina,
                PageSize = TamanhoPagina,
                PageCount = (Int32)TotalPaginas,
                Type = typeof(T).Name,
            };
        }

        public static IQueryable<T> Filtrar<T>(IQueryable<T> Dados, String Campo, String Valor)
        {
            PropertyInfo? CampoObjeto = typeof(T).GetProperty(Campo);
            if (CampoObjeto == null)
            {
                throw new ValidationException("Campo não encontrado");
            }
            ParameterExpression Linha = Expression.Parameter(typeof(T), "row");
            MethodInfo Comparador = (((Expression<Func<String, Boolean>>)(s => s.Contains(""))).Body as MethodCallExpression).Method;

            Expression Expressao = Expression.Property(Linha, CampoObjeto);// x => x.(TIPO)

            if (CampoObjeto.PropertyType.IsGenericType && CampoObjeto.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Expressao = Expression.NotEqual(Expressao, Expression.Constant(null, typeof(DateTime?)));
            }
            //if (CampoObjeto.PropertyType.IsValueType)
            //{
            //    Expressao = Expression.Property(Expressao, "Value");
            //}
            if (CampoObjeto.PropertyType != typeof(String))
            {
                if (CampoObjeto.PropertyType == typeof(DateTime))
                {
                    MethodInfo ConverterParaString = typeof(DateTime).GetMethod("ToShortDateString");
                    Expressao = Expression.Call(Expressao, ConverterParaString);
                }
                else if (CampoObjeto.PropertyType == typeof(DateTime?))
                {
                    // NAO FUNCIONA COMPLETAMENTE
                    MethodInfo ConverterParaString = typeof(DateTime).GetMethod("ToShortDateString");
                    Expression CondicaoConverter = Expression.Call(Expression.Property(Expression.Property(Linha, CampoObjeto), "Value"), ConverterParaString);
                    Expressao = Expression.AndAlso(Expressao, CondicaoConverter);
                }
                else if (CampoObjeto.PropertyType == typeof(Boolean))
                {
                    MethodInfo ConverterParaString = typeof(Object).GetMethod("ToString");
                    Expressao = Expression.Call(Expressao, ConverterParaString);
                    MethodInfo Maisculo = typeof(String).GetMethods().FirstOrDefault(x => x.Name == "ToUpper");
                    Expressao = Expression.Call(Expressao, Maisculo);
                    Valor = Valor.ToUpper();
                }
                else
                {
                    MethodInfo ConverterParaString = typeof(Object).GetMethod("ToString");

                    Expressao = Expression.Call(Expressao, ConverterParaString);
                }
            }
            else
            {
                MethodInfo Maisculo = typeof(String).GetMethods().FirstOrDefault(x => x.Name == "ToUpper");
                Expressao = Expression.Call(Expressao, Maisculo);
                Valor = Valor.ToUpper();
            }

            Func<T, Boolean> func =
                Expression.Lambda<Func<T, Boolean>>
                (
                    Expression.Call
                    (
                        Expressao,
                        Comparador,
                        Expression.Constant(Valor)
                    ),
                    Linha
                ).Compile();

            return Dados.Where(func).AsQueryable();
        }

        public static IOrderedQueryable<T>? Ordenar<T>(IQueryable<T> Dados, String Campo, Boolean Ordem = false)
        {
            Type Classe = typeof(T);
            ParameterExpression arg = Expression.Parameter(Classe, "x");
            Expression Expressao = arg;

            PropertyInfo? Propriedade = Classe.GetProperty(Campo);
            if (Propriedade == null)
            {
                throw new ValidationException("Campo não encontrado");
            }
            Expressao = Expression.Property(Expressao, Propriedade);
            Classe = Propriedade.PropertyType;

            String Ordenacao = Ordem ? "OrderBy" : "OrderByDescending";

            Type TipoDelegado = typeof(Func<,>).MakeGenericType(typeof(T), Classe);
            LambdaExpression lambda = Expression.Lambda(TipoDelegado, Expressao, arg);

            Object? result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == Ordenacao
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), Classe)
                    .Invoke(null, new object[] { Dados, lambda });
            return result as IOrderedQueryable<T>;
        }
    }
}
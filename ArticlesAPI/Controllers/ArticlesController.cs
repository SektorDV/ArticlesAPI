using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using ArticlesAPI.Models;

namespace ArticlesAPI.Controllers
{
    public class ArticlesController : ApiController
    {
        public static Dictionary<int, Article> dict = new Dictionary<int, Article>(); // mock database
        public static int currid = 4; // mock database index
        public static void CreateMockDB()
        {
            dict.Add(1, new Article("Article One", "Lorem ipsum", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", 1, new List<int> { 1, 3, 5 }, true));
            dict.Add(2, new Article("Article Two", "Bacon ipsum", "Bacon ipsum dolor amet ground round capicola bresaola, kielbasa short ribs cow shoulder alcatra flank. Tongue cow tenderloin spare ribs. Filet mignon pork belly jerky burgdoggen, cupim meatball turducken. Boudin ham strip steak doner pastrami, filet mignon pork belly fatback short ribs pig swine buffalo chuck bresaola.", 5, new List<int> { 2, 4 }, false));
            dict.Add(3, new Article("Article Three", "Cupcake ipsum", "Soufflé muffin sugar plum brownie soufflé. Topping apple pie caramels. Cake tart soufflé fruitcake chocolate cake croissant. Bonbon lemon drops candy canes. Sugar plum oat cake cake macaroon gummies. Brownie topping jelly-o pudding icing. Bonbon pie jelly. Chocolate cake gummi bears gummi bears gingerbread danish liquorice. Ice cream ice cream wafer sesame snaps cheesecake icing tart liquorice. Cheesecake gummi bears marzipan liquorice. Danish sweet brownie. Chocolate jelly-o candy canes pie chocolate cake fruitcake. Cheesecake fruitcake cupcake cupcake cupcake liquorice tootsie roll danish. Powder jelly chocolate cake candy canes.", 3, new List<int> { 1, 5 }, false));
        }

        /// <summary>
        /// Pobiera ostatni artykuł z bazy danych.
        /// </summary>
        /// <returns>Wartości pól artykułu</returns>
        public Article Get()
        {
            return dict[dict.Count];
        }

        /// <summary>
        /// Pobiera artykuł o określonym id z bazy danych.
        /// </summary>
        /// <param name="id">Id artykułu do wyświetlenia</param>
        /// <returns>Wartości pól artykułu</returns>
        public Article Get(int id)
        {
            return dict[id];
        }

        /// <summary>
        /// Dodaje artykuł do bazy danych.
        /// </summary>
        /// <param name="article">JSON z wartościami pól artykułu</param>
        /// <returns>OK</returns>
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody]Article article)
        {
            dict.Add(currid, article);
            currid++;
            return request.CreateResponse(HttpStatusCode.OK, "Article added");
        }

        /// <summary>
        /// Edytuje artykuł o określonym id w bazie danych.
        /// </summary>
        /// <param name="id">Id artykułu do edycji</param>
        /// <param name="article">JSON z określonymi wartościami pól artykułu</param>
        /// <returns>OK jeśli zedytowano, 404 jeśli nie znaleziono artykułu</returns>
        public HttpResponseMessage Put(HttpRequestMessage request, int id, [FromBody]Article article)
        {
            if (!dict.ContainsKey(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.NotFound, "Article not found");
            }
            PropertyInfo[] pi = typeof(Article).GetProperties();
            foreach(PropertyInfo property in pi)
            {
                if (property.GetValue(article) != null)
                {
                    property.SetValue(dict[id], property.GetValue(article));
                }
            }
            return request.CreateResponse(HttpStatusCode.OK, "Article edited");
        }


        /// <summary>
        /// Usuwa artykuł z określonym id z bazy danych.
        /// </summary>
        /// <param name="id">Id artykułu do usunięcia</param>
        /// <returns>OK jeśli usunięto, 404 jeśli nie znaleziono artykułu</returns>
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            if (!dict.ContainsKey(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.NotFound, "Article not found");
            }
            dict.Remove(id);
            return request.CreateResponse(HttpStatusCode.OK, "Article deleted");
        }

        /// <summary>
        /// Publikuje albo cofa publikację artykułu o określonym ID
        /// </summary>
        /// <param name="id">Id artykułu do publikacji/cofnięcia publikacji</param>
        /// <returns>OK i informację o publikacji, albo 404 jeśli artykuł nie istnieje</returns>
        [HttpGet]
        public HttpResponseMessage Publish(HttpRequestMessage request, int id)
        {
            if (!dict.ContainsKey(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.NotFound, "Article not found");
            }

            if (dict[id].Published != true)
            {
                dict[id].Published = true;
                return request.CreateResponse(HttpStatusCode.OK, "Article published");
            } else
            {
                dict[id].Published = false;
                return request.CreateResponse(HttpStatusCode.OK, "Article unpublished");
            }

        }

    }
}

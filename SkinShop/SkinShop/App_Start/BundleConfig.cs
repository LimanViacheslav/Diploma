using System.Web;
using System.Web.Optimization;

namespace SkinShop
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css", "~/Content/SkinShopCSS/Index.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/Index.css").Include(
                     "~/Content/SkinShopCSS/Index.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/SkinDetails.css").Include(
                      "~/Content/SkinShopCSS/SkinDetails.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/Layout.css").Include(
                     "~/Content/SkinShopCSS/Layout.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/PageNotFound.css").Include(
                     "~/Content/SkinShopCSS/PageNotFound.css"));

            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/Basket.css").Include(
                     "~/Content/SkinShopCSS/Basket.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/Orders.css").Include(
                     "~/Content/SkinShopCSS/Orders.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/Favorites.css").Include(
                     "~/Content/SkinShopCSS/Favorites.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/Users.css").Include(
                                 "~/Content/SkinShopCSS/Users.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/UserInfo.css").Include(
                                "~/Content/SkinShopCSS/UserInfo.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/Main.css").Include(
                               "~/Content/SkinShopCSS/Main.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/GameDetails.css").Include(
                               "~/Content/SkinShopCSS/GameDetails.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/GameDetails.css").Include(
                               "~/Content/SkinShopCSS/GameDetails.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/Register.css").Include(
                               "~/Content/SkinShopCSS/Register.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/Login.css").Include(
                               "~/Content/SkinShopCSS/Login.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/SuccessRegister.css").Include(
                               "~/Content/SkinShopCSS/SuccessRegister.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/CreateGame.css").Include(
                               "~/Content/SkinShopCSS/CreateGame.css"));
            bundles.Add(new StyleBundle("~/Content/SkinShopCSS/CreateSkin.css").Include(
                               "~/Content/SkinShopCSS/CreateSkin.css"));
        }
    }
}

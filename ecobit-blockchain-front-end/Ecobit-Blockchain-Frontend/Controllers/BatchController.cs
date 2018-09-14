using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ecobit_Blockchain_Frontend.Factories;
using Ecobit_Blockchain_Frontend.DataAccess.Interfaces;
using Ecobit_Blockchain_Frontend.Models;
using Ecobit_Blockchain_Frontend.Utils;
using Ecobit_Blockchain_Frontend.Utils.Auth;
using Ecobit_Blockchain_Frontend.Models.View;

namespace Ecobit_Blockchain_Frontend.Controllers
{
    [AuthenticationFilterAttribute]
    public class BatchController : Controller
    {
        private readonly ITransactionDao _transactionDao;
        private readonly IAuthenticationService _authService;

        public BatchController() : this(DependencyFactory.Resolve<ITransactionDao>(), DependencyFactory.Resolve<IAuthenticationService>())
        {
            
        }

        public BatchController(ITransactionDao transactionDao, IAuthenticationService authService)
        {
            _transactionDao = transactionDao;
            _authService = authService;
        }

        /// <summary>
        /// Returns index page with all controller function for transactions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// Posts a Batch model to index and redirects it to the right page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(Batch model)
        {
            return !ModelState.IsValid ? (ActionResult) View(model) : RedirectToAction("History", new
            {
                id = model.BatchId
            });
        }

        /// <summary>
        /// Returns a history of transactions for a batch id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult History(int id)
        {
            var transactions = _transactionDao.GetTransactionsByBatchId(id);

            var supplyChain = SupplyChainFactory.Make(transactions);

            var model = new HistoryView
            {
                BatchId = id,
                SupplyChain = supplyChain
            };
            
            return View("History", model);
        }

        [HttpGet]
        public ActionResult UserHistory()
        {
            var cookie = HttpContext.Request.Cookies.Get(AuthenticationFilterAttribute.CookieName);
            return UserHistory(cookie);
        }
        
        
        /// <summary>
        /// Shows a page with transactions for the user
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public ActionResult UserHistory(HttpCookie cookie)
        {
            User userData = _authService.GetUserData(cookie);
            
            var transactions = _transactionDao.GetTransactionsByUser(userData.Companyname);
            transactions = transactions.OrderBy(t => t.OrderTime).ToList();

            var model = new UserHistory
            {
                User = userData.Companyname,
                Transactions = transactions
            };
            
            return View("UserHistory", model);
        }
        
        
        /// <summary>
        /// returns a batch creation page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
        /// <summary>
        /// Saves a transaction to the blockchain
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("InvalidCreateTransaction", "Gegevens niet correct ingevuld.");
                return View(transaction);
            }
            
            _transactionDao.SaveTransaction(transaction);
            return View("Index");
        }
    }
}
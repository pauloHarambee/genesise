using GenesisExchange.Controllers.sub_controllers;
using GenesisExchange.Data;
using GenesisExchange.Models;
using GenesisExchange.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GenesisExchange.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(AppDbContext appDbContext) : base(appDbContext){ 
        }
        public IActionResult Index()
        {
            var Transactions = _appDbContext.Transactions.Include(s=>s.Beneficiaries).ToList();
            return View(new IndexViewModel
            {
                PendingCount = Transactions.Select(s => s.Status == "Pending").Count(),
                TransactionCount = Transactions.Count(),
                Transactions = Transactions.OrderBy(s=>s.Id).Take(5),
                TransactionTotal = Transactions.Select(s => s.AmountZAR).Sum()
            }) ;
        }
        [HttpGet]
        public IActionResult Statements()
        {
            return View(_appDbContext.Transactions
                .Include(s => s.Beneficiaries)
                .ThenInclude(x => x.Bank).ToList()) ;
        }
        [HttpGet]
        public IActionResult PaymentPortal(int? beneficiaryId)
        {
            return beneficiaryId != null ? View(new PaymentViewModel { 
                AccountNumber = _appDbContext.Beneficiaries.Find(beneficiaryId).AccountNumber}) 
                : View();
        }
        [HttpPost]
        public IActionResult PaymentPortal(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!BeneficiaryExists(model.AccountNumber))
            {
                ModelState.AddModelError("", $"Beneficiary with Account Number: {model.AccountNumber} doesn't exists! Please first add new beneficiary!");
                return View(model);
            }
            try
            {
                decimal ethopiaAmount = (model.Amount * (decimal)model.Rate);
                _appDbContext.Transactions.Add(new Transactions
                {
                    TransactionDate = DateTime.UtcNow,
                    Status = "Pending",
                    Amount = ethopiaAmount,
                    AmountZAR = model.Amount,
                    Beneficiaries = FindBeneficiaryByAccount(model.AccountNumber),
                    Rate = model.Rate
                });
                if (_appDbContext.SaveChanges() > 0)
                    return RedirectToAction(nameof(PaymentSuccess), new { AccountNumber = model.AccountNumber, Amount = model.Amount });

                ModelState.AddModelError("", $"Failed to send Payment to account number: {model.AccountNumber}! Please try again!");
                return View(model);
            }
            catch {
                ModelState.AddModelError("", $"Failed to send Payment to account number: {model.AccountNumber}! Please try again!");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult PaymentSuccess(string AccountNumber, string Amount)
        {
            return View(new PaymentSuccessViewModel
            {
                Amount = Amount,
                Account = AccountNumber,
                Status = "Pending"
            });
        }
        [HttpGet]
        public IActionResult AddBeneficiary() { 
            PopulateDDL();
            return View();
        }
        [HttpPost]
        public IActionResult AddBeneficiary(BeneficiaryViewModel model)
        {
            if(!ModelState.IsValid)
            {
                PopulateDDL();
                return View(model);
            }
            if (BeneficiaryExists(model.AccountNumber))
            {
                ModelState.AddModelError("", $"Beneficiary with Account Number: {model.AccountNumber} Already exists");
                PopulateDDL();
                return View(model);
            }
            try
            {
                _appDbContext.Beneficiaries.Add(new Models.Beneficiaries
                {
                    AccountNumber = model.AccountNumber,
                    FullName = model.FullName,
                    Sender = model.Sender,
                    SenderNumber = model.SenderNumber,
                    Bank = FindBankById(model.BankId),
                });
                if(_appDbContext.SaveChanges()> 0)
                    return RedirectToAction(nameof(Beneficiaries), new { Message = $"Beneficiary with Account Number: {model.AccountNumber} was created successful" });

                ModelState.AddModelError("", $"Failed to create Beneficiary with account number: {model.AccountNumber}! Please try again!");
                PopulateDDL();
                return View(model);
            }
            catch {
                ModelState.AddModelError("", $"Failed to create Beneficiary with account number: {model.AccountNumber}! Please try again!");
                PopulateDDL();
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Beneficiaries(string Message)
        {
            if(!string.IsNullOrEmpty(Message))
                ViewBag.Message = Message;  

            return View(_appDbContext.Beneficiaries.OrderBy(s=>s.Id).Include(s=>s.Bank));
        }
        private Bank FindBankById(int bankId)
        {
            return _appDbContext.Banks.Find(bankId);
        }

        private Beneficiaries FindBeneficiaryByAccount(string accountNumber)
        {
            return _appDbContext.Beneficiaries.FirstOrDefault(s=> s.AccountNumber == accountNumber); 
        }
        private bool BeneficiaryExists(string accountNumber)
        {
            return _appDbContext.Beneficiaries.Any(s=>s.AccountNumber == accountNumber);
        }

        private void PopulateDDL()
        {
            ViewBag.BankId = new SelectList(_appDbContext.Banks.ToList(), "Id", "Name");
        }
    }
}

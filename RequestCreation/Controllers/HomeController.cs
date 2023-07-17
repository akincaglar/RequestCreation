using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequestCreation.Context;
using RequestCreation.Models;
using System.Diagnostics;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace RequestCreation.Controllers
{


    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RequestDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, RequestDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Home"); // Controller adını uygun şekilde değiştirin
        }

        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _dbContext.User.FirstOrDefault(u => u.UserName == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    //HttpContext.Session.SetString("UserId", user.Id.ToString());
                    //HttpContext.Session.SetString("Username", user.UserName);

                    return RedirectToAction("Index", "Home"); // Başarılı giriş durumunda yönlendirilecek sayfa
                }
            }

            ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre");
            return View(model); 
        }



        //public bool Delete(int Id)
        //{
        //    return _dbContext.Requests.(Id);
        //}


        public IActionResult Index()
        {
            //var userId = HttpContext.Session.GetString("UserId");
            //var username = HttpContext.Session.GetString("Username");

            ViewData["getStatusList"] = _dbContext.Status.ToList();
            var list = (
                from r in _dbContext.Requests
                join n in _dbContext.Notes on r.Id equals n.RequestsId into notes
                from note in notes.DefaultIfEmpty()
                join u in _dbContext.User on r.CreatedBy equals u.Id
                join s in _dbContext.Status on r.StatusId equals s.Id
                //orderby r.Id descending
                select new RequestDetail
                {
                    Id = r.Id,
                    Request = r.Request,
                    CitizenName = r.CitizenName,
                    CitizenPhone = r.CitizenPhone,
                    CreatedName = u.Name + ' ' + u.Surname,
                    CreatedBy = u.Id,
                    CreatedOn = r.CreatedOn,
                    StatusName = s.StatusName,
                    StatusId = s.Id,
                    Note = note != null ? note.Note : string.Empty
                }).ToList();
            list.Reverse();
            //list = list.OrderByDescending(r => r.Id).ToList();
            return View(list);
        }

      
        [HttpPost]
        public ActionResult Save([FromBody] RequestDetail model)
        {
            if (ModelState.IsValid)
            {
                var data = new Requests
                {
                    Request = model.Request,
                    CitizenName = model.CitizenName,
                    CitizenPhone = model.CitizenPhone,
                    StatusId = 1,
                    CreatedBy = 3,
                    CreatedOn = DateTime.Now
                };
                _dbContext.Requests.Add(data);
                _dbContext.SaveChanges();

                var data2 = new Notes
                {
                    Note = model.Note,
                    RequestsId = data.Id,
                    CreatedOn = DateTime.Now,
                    CreatedBy = 3
                };
                _dbContext.Notes.Add(data2);
                _dbContext.SaveChanges();

                return Ok();
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                  .Select(e => e.ErrorMessage)
                                  .ToList();
            return BadRequest();
        }

      
        [HttpPost]
        public void Update(RequestDetail requestModel)
        {
            try
            {
                var item = _dbContext.Requests.FirstOrDefault(x => x.Id == requestModel.Id);

                item.Request = requestModel.Request;
                item.CitizenName = requestModel.CitizenName;
                item.CitizenPhone = requestModel.CitizenPhone;
                item.StatusId = requestModel.StatusId;
                item.CreatedBy = item.CreatedBy;
                item.CreatedOn = item.CreatedOn;

                _dbContext.Requests.Update(item);
                _dbContext.SaveChanges();

                var item2 = _dbContext.Notes.FirstOrDefault(x => x.RequestsId == requestModel.Id);

                if (item2 == null)
                {
                    var newNote = new Notes
                    {
                        Note = requestModel.Note,
                        RequestsId = requestModel.Id,
                        CreatedOn = DateTime.Now,
                        CreatedBy = 3
                    };

                    _dbContext.Notes.Add(newNote);
                }
                else
                {
                    item2.Note = requestModel.Note;
                    item2.CreatedOn = item2.CreatedOn;
                    item2.CreatedBy = item2.CreatedBy;
                    item2.RequestsId = item.Id;

                    _dbContext.Notes.Update(item2);
                }
                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet]
        public IActionResult GetDetails(int id)
        {
            if (id != null)
            {

                var result =
                (
                    from r in _dbContext.Requests
                    join n in _dbContext.Notes on r.Id equals n.RequestsId into notes
                    from note in notes.DefaultIfEmpty()
                    join u in _dbContext.User on r.CreatedBy equals u.Id
                    join s in _dbContext.Status on r.StatusId equals s.Id
                    select new RequestDetail
                    {
                        Id = r.Id,
                        Request = r.Request,
                        CitizenName = r.CitizenName,
                        CitizenPhone = r.CitizenPhone,
                        CreatedName = u.Name + ' ' + u.Surname,
                        StatusName = s.StatusName,
                        StatusId = s.Id,
                        Note = note != null ? note.Note : string.Empty
                    }).Where(x => x.Id == id).FirstOrDefault();
                return Json(result);
            }
            return NotFound();
        }

        public JsonResult GetStatus()
        {
            var list = _dbContext.Status.ToList();

            return Json(list);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (model.Username == "akin" && model.Password == "Ak1453")
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");

        //    return View(model);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    return RedirectToAction("Login", "Home");
        //}


    }
}
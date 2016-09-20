using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameLobbyServer.Models;
using System.Text;

namespace GameLobbyServer.Controllers
{
    public class RoomsController : Controller
    {
        private RoomDBContext db = new RoomDBContext();

        private char roomListStringSeparator = '#';

        public string RoomList()
        {
            StringBuilder s = new StringBuilder();
            //e.g 1#player#room name#1#10#127.0.0.1##
            foreach (var r in db.Rooms.ToList())
            {
                s.Append(r.ID.ToString());
                s.Append(roomListStringSeparator);
                s.Append(r.HostPlayerName.ToString());
                s.Append(roomListStringSeparator);
                s.Append(r.Description.ToString());
                s.Append(roomListStringSeparator);
                s.Append(r.CurrentPlayer.ToString());
                s.Append(roomListStringSeparator);
                s.Append(r.MaxPlayer.ToString());
                s.Append(roomListStringSeparator);
                s.Append(r.HostIP.ToString());
                s.Append(roomListStringSeparator);

                s.Append(roomListStringSeparator);
            }
            return s.ToString();
        }

        public string IPAddress()
        {
            string IPAdd = string.Empty;
            IPAdd = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(IPAdd))
                IPAdd = Request.ServerVariables["REMOTE_ADDR"];//在hostease上，此处获取到的是server ip
            /*
            foreach (var item in Request.ServerVariables)
            {
                IPAdd += item.ToString() + ": " + Request.ServerVariables[item.ToString()];
                IPAdd += "<br />";
            }
            */
            IPAdd = IPAdd.Split(',')[0];
            IPAdd = IPAdd.Split(':')[0];
            return IPAdd;
        }

        // GET: Rooms
        public ActionResult Index()
        {
            return View(db.Rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //有这个属性unity处无法提交表单，因为没有该值，验证不通过
        //[ValidateAntiForgeryToken]
        public string Create([Bind(Include = "ID,HostPlayerName,Description,CurrentPlayer,MaxPlayer,HostIP")] Room room)
        {
            if (ModelState.IsValid)
            {
                room.HostIP = IPAddress();
                db.Rooms.Add(room);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return room.ID.ToString();
            }

            //return View(room);
            return "error";
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,HostPlayerName,Description,CurrentPlayer,MaxPlayer,HostIP")] Room room)
        {
            if (ModelState.IsValid)
            {
                room.HostIP = IPAddress();
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public string DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return id.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

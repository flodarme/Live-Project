using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScheduleIt2.Models;

namespace ScheduleIt2.Controllers
{
    public class WorkTimeEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WorkTimeEvents
        [Authorize]
        public ActionResult Index()
        {
            return View(db.WorkTimeEvents.ToList());
        }

        // GET: WorkTimeEvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WorkTimeEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,Start,End,Note,Title,ActiveSchedule,ApproverId,Id,ClockFunctionStatus")] WorkTimeEvent workTimeEvent)
        {
            if (ModelState.IsValid)
            {
                //Changed the Start parameter so it is set to current time.
                workTimeEvent.Start = DateTime.Now;
                workTimeEvent.EventID = Guid.NewGuid();
                db.WorkTimeEvents.Add(workTimeEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workTimeEvent);
        }

        // GET: WorkTimeEvents/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTimeEvent workTimeEvent = db.WorkTimeEvents.Find(id);
            if (workTimeEvent == null)
            {
                return HttpNotFound();
            }
            return View(workTimeEvent);
        }

        // POST: WorkTimeEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,Start,End,Note,Title,ActiveSchedule,ApproverId,Id,ClockFunctionStatus")] WorkTimeEvent workTimeEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workTimeEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(workTimeEvent);
        }

        // GET: WorkTimeEvents/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkTimeEvent workTimeEvent = db.WorkTimeEvents.Find(id);
            if (workTimeEvent == null)
            {
                return HttpNotFound();
            }
            return View(workTimeEvent);
        }

        // POST: WorkTimeEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            WorkTimeEvent workTimeEvent = db.WorkTimeEvents.Find(id);
            db.WorkTimeEvents.Remove(workTimeEvent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ClockToggle(int? userID)
        {
            //When Event model/seed is updated, remove x.ID == filler and
            //Use x.Id == userID
            var clockState = db.WorkTimeEvents
                .Where(x => x.End == null &&
                //x.Id == userID 
                x.Id == "filler"
                ).ToList();
            if(clockState.Count() > 0)
            {
                clockState[0].End = DateTime.Now;
            } else
            {
                db.WorkTimeEvents.Add(new WorkTimeEvent()
                {
                    //When Event model/seed is updated, add Id = userID
                    Start = DateTime.Now,
                    End = null,
                    ClockFunctionStatus = ClockFunctionStatus.ClockInSuccess
                    //Id = userID
                });
            }
            return View();
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

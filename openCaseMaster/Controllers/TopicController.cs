﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using openCaseMaster.Models;
using openCaseMaster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace openCaseMaster.Controllers
{
    [Authorize(Roles = "user")]
    public class TopicController : Controller
    {

        [AllowAnonymous]
        public ActionResult Index(int page=1)
        {
         
            QCTESTEntities QC_DB = new QCTESTEntities();

            var lsv = from t in QC_DB.topic
                      where t.state != 0
                      orderby t.ID descending
                      select new taskModel_prev
                      {
                          ID = t.ID,
                          title = t.title,
                          nodeID = t.node,
                          userName = t.admin_user.Username,
                          creatDate = t.creatDate,
                          scriptCount = t.M_publicTask.Sum(tk=>tk.M_publicTaskScript.Count)
                      };

         
            ViewBag.select = "Index";
            ViewBag.page = page;
            int rows = 15;


            var v = lsv.Skip(rows * (page - 1)).Take(rows).ToList();
         
        
            return View(v);
        }

        [HttpGet]
        public ActionResult add()
        {

            ViewBag.nodes = this.NodesList();
            if (userHelper.isAdmin)
            {
                appSelectItem();
                return View("adminAdd");
            }
            else
                return View("add");
        }

        /// <summary>
        /// 编辑普通节点
        /// </summary>
        [HttpGet]
        public ActionResult edit(int id)
        {
            QCTESTEntities QC_DB = new QCTESTEntities();

            var tic = QC_DB.topic.First(t => t.ID == id && t.state != 0 && t.node > 200);


            topicModel tm = new topicModel();
            tm.node = tic.node;
            tm.title = tic.title;
            tm.body = tic.body;

            var nodes = this.PublicNodes();

            foreach(var n in nodes)
            {
                if(n.Value == tic.node.ToString())
                {
                    n.Selected = true;
                    break;
                }
            }

            ViewBag.nodes = nodes;

            return View("edit", tm);

        }


        /// <summary>
        /// 编辑任务节点
        /// </summary>
        [HttpGet]
        public ActionResult editTask(int id)
        {
            QCTESTEntities QC_DB = new QCTESTEntities();

            var tic = QC_DB.topic.First(t => t.ID == id && t.state != 0 && t.node < 200);


            topicModel tm = new topicModel();
            tm.node = tic.node;
            tm.title = tic.title;
            tm.body = tic.body;

            var node =  topicHelper.nodes.First(t => t.Key == tic.node);

            List<SelectListItem> ls = new List<SelectListItem>();
            ls.Add(new SelectListItem { Text = node.Value, Value = node.Key.ToString(), Selected = true });
            
          
            ViewBag.nodes = ls;

            return View("editTask", tm);

        }

        /// <summary>
        /// 编辑普通节点
        /// </summary>
        [HttpPost]
        public ActionResult edit(topicModel tm)
        {
            QCTESTEntities QC_DB = new QCTESTEntities();
            
            var tic = QC_DB.topic.First(t => t.ID == tm.ID && t.state != 0 && t.node > 200);
            tic.node = tm.node;
            tic.title = tm.title;
            tic.body = tm.body;

            QC_DB.SaveChanges();

            return RedirectToAction(tm.ID.ToString());

        }

       
        [HttpPost]
        public ActionResult add(topicModel tm)
        {
            
          
            if (!ModelState.IsValid)//验证模型
            {
                ViewBag.nodes = this.NodesList();
                return View(tm);
            }

           

            //开始操作

            QCTESTEntities QC_DB = new QCTESTEntities();

            topic pt = new topic();

            pt.node = tm.node;
            pt.creatDate = DateTime.Now;
            pt.title = tm.title;
            pt.body = tm.body;
            pt.userID = userHelper.getUserID();

            QC_DB.topic.Add(pt);

            
            QC_DB.SaveChanges();

            TempData["event"] = "add";

            return RedirectToAction(pt.ID.ToString());
            
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult adminAdd(topicModel_taskAdd tm)
        {
      

            if (!ModelState.IsValid)//验证模型
            {
                appSelectItem();
                ViewBag.nodes = this.NodesList();
                return View(tm);
            }

            //开始操作

            QCTESTEntities QC_DB = new QCTESTEntities();

            topic tp = new topic();

            tp.creatDate = DateTime.Now;
            tp.title = tm.title;
            tp.body = tm.body;
            tp.userID = userHelper.getUserID();
            tp.node = tm.node;

            QC_DB.topic.Add(tp);

            //解析json,添加script
            if (tm.node == 101)
            {
                M_publicTask pt = new M_publicTask();
                pt.topicID = tp.ID;
                pt.appID = tm.appID;
                pt.creatDate = DateTime.Now;
                pt.startDate = tm.startDate;
                pt.endDate = tm.endDate;

                QC_DB.M_publicTask.Add(pt);

                var ja = JArray.Parse(tm.scripts);
                foreach (var j in ja.Children<JObject>())
                {

                    int ID = Convert.ToInt32(j["ID"].ToString());
                    var tmp = QC_DB.tmp_TaskScript.FirstOrDefault(t => t.ID == ID);
                    if (tmp == null) continue;

                    M_publicTaskScript ts = new M_publicTaskScript();
                    ts.taskID = pt.ID;
                    ts.title = tmp.title;
                    ts.script = tmp.script;

                    QC_DB.M_publicTaskScript.Add(ts);
                    QC_DB.tmp_TaskScript.Remove(tmp);
                }
            }

            QC_DB.SaveChanges();

            TempData["event"] = "add";

            return RedirectToAction(tp.ID.ToString());

        }

        /// <summary>
        /// 初始化 可选app
        /// </summary>
        [NonAction]
        private void appSelectItem()
        {
            SelectListGroup slg = new SelectListGroup();
            slg.Name = "可选应用";

            var apps = (from t in userHelper.getApps()
                        select new SelectListItem
                        {
                            Text = t.name,
                            Value = t.ID.ToString()
                        }).ToList();


            apps.ForEach(t => t.Group = slg);
            apps.Insert(0, new SelectListItem { Text = "请选择被测应用", Value = "" });
            ViewBag.apps = apps;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("topic/{id:int}")]
        public ActionResult Topic(int id)
        {
            if (TempData["event"] != null)
                ViewBag.clear = true;
            taskModel_view tv = new taskModel_view(id);

            return View(tv);
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            QCTESTEntities QC_DB = new QCTESTEntities();

            var ts = QC_DB.topic.First(t => t.ID == id);
            if (ts.userID != userHelper.getUserID())
                return RedirectToAction("Index");
            ts.state = 0;
            QC_DB.SaveChanges();
            return RedirectToAction("Index");
        }





        /// <summary>
        /// 用户可用节点
        /// </summary>
        [NonAction]
        private List<SelectListItem> NodesList(int id = 0)
        {
            List<SelectListItem> SLI = new List<SelectListItem>();
            SLI.Add(new SelectListItem { Text = "请选择节点", Value = "" });


            SLI.AddRange(PublicNodes());


            if (userHelper.isAdmin)
                SLI.AddRange(adminNodes());

            if (id < 1) return SLI;

            foreach (var s in SLI)
            {
                if (s.Value == id.ToString())
                {
                    s.Selected = true;
                    break;
                }
            }

            return SLI;

        }


        /// <summary>
        /// 普通节点
        /// </summary>
        [NonAction]
        private List<SelectListItem> PublicNodes()
        {
            List<SelectListItem> SLI = new List<SelectListItem>();
           

            SelectListGroup slg = new SelectListGroup();
            slg.Name = "可选节点";

            topicHelper.nodes.Where(t => t.Key > 200).ToList()
                .ForEach(n => SLI.Add(new SelectListItem { Text = n.Value, Value = n.Key.ToString(), Group = slg }));


            return SLI;

        }

        /// <summary>
        /// admin 节点
        /// </summary>
        [NonAction]
        private List<SelectListItem> adminNodes()
        {
            List<SelectListItem> SLI = new List<SelectListItem>();

            SelectListGroup slgTask = new SelectListGroup();
            slgTask.Name = "测试任务";

            topicHelper.nodes.Where(t => t.Key < 200).ToList()
                .ForEach(n => SLI.Add(new SelectListItem { Text = n.Value, Value = n.Key.ToString(), Group = slgTask }));


            return SLI;

        }
    }
}
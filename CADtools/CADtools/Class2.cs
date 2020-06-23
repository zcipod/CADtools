using System;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DNA;

namespace CADtools
{
    class Class2
    {
        public void Test(string dir1, string dir2, string TcName)
        {
            DocumentCollection acDocMgr = Application.DocumentManager;
            Database dbs = new Database();
            Database dbt = new Database();

            if (!System.IO.File.Exists(dir1) || !System.IO.File.Exists(dir2))
            {
                return;
            }
            Document doc1 = Application.DocumentManager.Open(dir1, false);
            Document doc2 = Application.DocumentManager.Open(dir2, false);
            dbs = doc1.Database;
            dbt = doc2.Database;

            ObjectIdCollection[] objcol;
            string[] LayoutName;
            int num = 0;

            using (DocumentLock lkdoc = doc1.LockDocument())
            {
                using (Transaction trans = dbs.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(dbs.BlockTableId, OpenMode.ForRead, false);
                    foreach (ObjectId btro in bt)
                    {
                        BlockTableRecord btr = (BlockTableRecord)trans.GetObject(btro, OpenMode.ForWrite);
                        if (!btr.IsLayout) continue;
                        Layout lot = (Layout)trans.GetObject(btr.LayoutId, OpenMode.ForRead);
                        if (lot.LayoutName.ToString() == "Model")continue;
                        
                        num = num + 1;
                    }

                    LayoutName = new string[num];
                    objcol = new ObjectIdCollection[num];
                    int i = 0;

                    foreach (ObjectId btro in bt)
                    {
                        BlockTableRecord btr = (BlockTableRecord)trans.GetObject(btro, OpenMode.ForWrite);
                        if (!btr.IsLayout) continue;
                        Layout lot = (Layout)trans.GetObject(btr.LayoutId, OpenMode.ForRead);
                        if (lot.LayoutName.ToString() == "Model") continue;

                        LayoutName[i] = lot.LayoutName.ToString();
                        ObjectIdCollection ooo = new ObjectIdCollection();
                        foreach (ObjectId ot in btr)
                        {
                            Entity ent = (Entity)trans.GetObject(ot, OpenMode.ForWrite);
                            string enttype = ent.GetType().ToString();
                            if (enttype == "Autodesk.AutoCAD.DatabaseServices.Viewport")
                            {
                                continue;
                            }
                            ooo.Add(ot);
                        }
                        objcol.SetValue(ooo, i);
                        i = i + 1;
                    }
                }
            }
            //   
            using (DocumentLock lkdoc = doc2.LockDocument())
            {
                using (Transaction trans = dbt.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(dbt.BlockTableId, OpenMode.ForRead, false);
                    //          acDocMgr.MdiActiveDocument = doc2;
                    //          BlockTableRecord btr1 = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.PaperSpace], OpenMode.ForWrite);
                    //          Layout lo1 = (Layout)trans.GetObject(btr1.LayoutId, OpenMode.ForRead);
                    //          LayoutManager.Current.CurrentLayout = lo1.LayoutName;
                    //          LayoutManager lom = LayoutManager.Current;
                    //          acDocMgr.DocumentActivationChanged; 
                    int i;
                    for (i = 0; i < num; i++)
                    {
                        foreach (ObjectId btro in bt)
                        {
                            BlockTableRecord btr = (BlockTableRecord)trans.GetObject(btro, OpenMode.ForRead);
                            if (!btr.IsLayout)
                            {
                                continue;
                            }
                            Layout lot = (Layout)trans.GetObject(btr.LayoutId, OpenMode.ForRead);
                            if (lot.LayoutName.ToString() == LayoutName[i])
                            {
                                IdMapping IdMap = new IdMapping();
                                dbt.WblockCloneObjects(objcol[i], btr.ObjectId, IdMap, DuplicateRecordCloning.Ignore, false);
                                break;
                            }
                        }
                    }
                    trans.Commit();
                }
            }
        }
    }
}

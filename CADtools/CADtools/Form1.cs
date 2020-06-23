using System;
//using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using acadAPP=Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DNA;


namespace CADtools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string dir1;
        public string dir2;
        public string TcName;
        private void button1_Click(object sender, EventArgs e)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = acadAPP.Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                while (true)
                {
                    PromptDoubleOptions xxx_pt = new PromptDoubleOptions("\n请输入需要画参考线的X坐标");
                    PromptDoubleResult xxx_pr = ed.GetDouble(xxx_pt);
                    if (xxx_pr.Status == PromptStatus.OK)
                    {
                        Double XXX = xxx_pr.Value;
                        Point3d pp1 = new Point3d(XXX, 0, 0);
                        Point3d pp2 = new Point3d(XXX, 1000, 0);
                        Xline xl1;
                        xl1 = new Xline();
                        xl1.BasePoint = pp1;
                        xl1.SecondPoint = pp2;
                        Tools.AddToModelSpace(xl1);
                    }
                    else
                    {
                        break;
                    }
                }
                tran.Commit();
            }
        }





        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = acadAPP.Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                while (true)
                {
                    PromptDoubleOptions yyy_pt = new PromptDoubleOptions("\n请输入需要画参考线的Y坐标");
                    PromptDoubleResult yyy_pr = ed.GetDouble(yyy_pt);
                    if (yyy_pr.Status == PromptStatus.OK)
                    {
                        Double YYY = yyy_pr.Value;
                        Point3d pp1 = new Point3d(0, YYY, 0);
                        Point3d pp2 = new Point3d(1000, YYY, 0);
                        Xline xl1;
                        xl1 = new Xline();
                        xl1.BasePoint = pp1;
                        xl1.SecondPoint = pp2;
                        Tools.AddToModelSpace(xl1);
                    }
                    else
                    {
                        break;
                    }
                }
                tran.Commit();
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Double qd =Convert.ToDouble(textBox1.Text);
            Double cslc = Convert.ToDouble(textBox2.Text);

            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = acadAPP.Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                while (true)
                {
                    PromptDoubleOptions lc_pt = new PromptDoubleOptions("\n请输入要寻找的桩号");
                    PromptDoubleResult lc_pr = ed.GetDouble(lc_pt);
                    if (lc_pr.Status == PromptStatus.OK)
                    {
                        Double lc = lc_pr.Value;
                        lc = lc - qd + cslc;
                        Point3d pp1 = new Point3d(lc, 0, 0);
                        Point3d pp2 = new Point3d(lc, 1000, 0);
                        Xline xl1;
                        xl1 = new Xline();
                        xl1.BasePoint = pp1;
                        xl1.SecondPoint = pp2;
                        Tools.AddToCurrentSpace(xl1);
                    }
                    else
                    {
                        break;
                    }
                }
                tran.Commit();
            }



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "450.679";
            textBox2.Text = "-372.9881";
            /*
//           FileInfo aFile = new FileInfo("Temp");
//            if (aFile.Exists)
//            {
            if (File.Exists("Temp"))
            {
                FileStream aFile = new FileStream("Temp", OpenMode.ForRead);

//              FileStream aFile=File.OpenRead("Temp");
                FileInfo aFileInfo=new FileInfo("Temp"):
                FileStream aFile=aFileInfo.OpenRead();
 此处列出另外两种以流形式打开文件的方式

            }
            else
            {

            }
            */
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Double qd = Convert.ToDouble(textBox1.Text);
            Double cslc = Convert.ToDouble(textBox2.Text);

            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = acadAPP.Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                while (true)
                {
                    PromptPointOptions lc_pt = new PromptPointOptions("\n请点选要寻找的桩号的点");
                    PromptPointResult lc_pr = ed.GetPoint(lc_pt);
                    if (lc_pr.Status == PromptStatus.OK)
                    {
                        Point3d lcp = lc_pr.Value;
                        Double lc = lcp.X+ qd - cslc;
                        lc = (int)lc;
                        DBText mt = new DBText();
                        mt.TextString = Convert.ToString(lc);
                        mt.Height = 5;
                        mt.Position = lcp;
                        Tools.AddToCurrentSpace(mt);
                    }
                    else
                    {
                        break;
                    }
                }
                tran.Commit();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = acadAPP.Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction tran = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tran.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord btr = (BlockTableRecord)tran.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                double area = 0;
                foreach (ObjectId ObjId in btr)
                {
                    Entity ent = (Entity)tran.GetObject(ObjId,OpenMode.ForWrite);
                    switch (ent.Layer.ToString().ToLower())
                    {
                        case "交通组织图-标线":
                            string enttype = ent.GetType().ToString();
                            if(enttype=="Autodesk.AutoCAD.DatabaseServices.Polyline")
                            {
                                Polyline pl = (Polyline)ent;
                                switch (pl.Linetype.ToLower())
                                {
                                    case "continuous":
                                        area = area + pl.Length * pl.ConstantWidth;
                                        break;
                                    case "byLayer":
                                        area = area + pl.Length * pl.ConstantWidth;
                                        break;
                                    case "减速让行线":
                                        area = area + pl.Length * pl.ConstantWidth*2/3;
                                        break;
                                    case "可跨越同向车道分界线":
                                        area = area + pl.Length * pl.ConstantWidth/3;
                                        break;
                                    case "港湾式停靠站线":
                                        area = area + pl.Length * pl.ConstantWidth/2;
                                        break ;
                                    default:
                                        break;
                                }


                                                                 
                            }
                            break;

                        case "交通组织图-标志":
                            string enttype1 = ent.GetType().ToString();
                            if (enttype1 == "Autodesk.AutoCAD.DatabaseServices.BlockReference")
                            {
                                BlockReference blkRf = (BlockReference)ent;
                                switch (blkRf.Name)
                                {
                                    case "箭头-左(右)拐弯-小":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 0.5568;
                                        break;
                                    case "箭头-左转或掉头":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 1.0659;
                                        break;
                                    case "箭头-左拐或右拐":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 1.0994;
                                        break;
                                    case "箭头-左(右)拐弯":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 0.6987;
                                        break;
                                    case "箭头-直行或掉头":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 0.9071;
                                        break;
                                    case "箭头-直行、左(右)拐弯":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 0.935;
                                        break;
                                    case "箭头-直行":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 0.54;
                                        break;
                                    case "箭头-掉头":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 1.3249;
                                        break;
                                    case "标志-人行道预警":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 1.1416;
                                        break;
                                    case "标志-减速让行":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 1.1626;
                                        break;
                                    case "标志-非机动车道":
                                        area = area + blkRf.ScaleFactors.X * blkRf.ScaleFactors.X * 0.0368;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;

                        case "交通组织图-标牌":
                            string enttype2 = ent.GetType().ToString();

                            break;

                        default:
                            break;
                    }
                    
                }
                textBox3.Text = area.ToString("F3");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dir1 = textBox4.Text;
            dir2 = textBox5.Text;
            TcName = textBox6.Text;
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "AutoCAD图形文件|*.dwg|所有文件|*.*";
            ofd.RestoreDirectory = true;
            ofd.FilterIndex = 1;
            if (ofd.ShowDialog() == DialogResult.OK) textBox4.Text = ofd.FileName;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "AutoCAD图形文件|*.dwg|所有文件|*.*";
            ofd.RestoreDirectory = true;
            ofd.FilterIndex = 1;
            if (ofd.ShowDialog() == DialogResult.OK) textBox5.Text = ofd.FileName;
        }
    }
}

using System;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DNA;

/*Copyright ©  2012

AutoCAD version:AutoCAD 2010
Description:  

To use CADtools.dll:

1. Start AutoCAD and open a new drawing.
2. Type netload and select CADtools.dll.
3. Execute the tt command.*/


namespace CADtools
{
    /// <summary>
    /// Summary for Class1.
    /// </summary>
    public class Class1
    {
        Database db = HostApplicationServices.WorkingDatabase;
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        [CommandMethod("tt")]
        public void tt()
        {
            Form1 ff1 = new Form1();
            Application.ShowModalDialog(ff1);
            string d1 = ff1.dir1;
            string d2 = ff1.dir2;
            string tn = ff1.TcName;
            Class2 cl2=new Class2();
            cl2.Test(d1,d2,tn);
        }




    }
}

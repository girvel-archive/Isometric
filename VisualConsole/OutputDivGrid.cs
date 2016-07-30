using System;
using VectorNet;

namespace VisualConsole
{
    [Serializable]
    public class OutputDivGrid : OutputDiv {
        public IGridWritable WriteSubject { get; set; }
        public IntVector WriteSubjectOffset { get; set; }

        public OutputDivGrid(IntVector begin, IntVector end, ConsoleIface iface, IGridWritable subject)
                : base(begin, end, iface, "") {
            WriteSubject = subject;
            WriteSubjectOffset = new IntVector(0, 0);
        }

        public override void Refresh() {
                Clear();
                for (var y = 0; 
                    y < Math.Min(
                    End.Y - Begin.Y, 
                    WriteSubject.SizeY - WriteSubjectOffset.Y); 
                    y++) {

                    for (var x = 0; 
                        x < Math.Min(
                        End.X - Begin.X, 
                        WriteSubject.SizeX - WriteSubjectOffset.X); 
                        x++) {

                        var currentSubject = WriteSubject[new IntVector(
                            x + WriteSubjectOffset.X,
                            y + WriteSubjectOffset.Y)];

                        ConsoleWriteHelper.Write(
                            currentSubject.Character.ToString(), 
                            Begin.X + x, 
                            Begin.Y + y,
                            currentSubject.Color);
                    }
                }
        }
    }
}
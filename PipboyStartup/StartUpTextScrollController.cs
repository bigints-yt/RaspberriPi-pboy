using Godot;
using System;

namespace RaspberriPipboy.PipboyStartup {
    
    [GlobalClass]
    public partial class StartUpTextScrollController : Control {
        
        [Export] RichTextLabel textLabel;

        private const int INTRO_LINE_LENGTH = 32;
        private const int GENERATION_COUNT = 128;
        private const int OUTRO_LINE_LENGTH = 64;
        private const double LINE_DELAY_BASE = 0.05;
        private const double LINE_VARIATION = 0.025;
        
        private int introIndex { get; set; } = 0;
        private int generationIndex { get; set; } = 0;
        private int outroIndex { get; set; } = 0;
        
        private double lineDelay { get; set; } = 0.0;
        private double lineTimer { get; set; } = 0.0;
        private string startUpText { get; set; } = "";

        
        // ========================================= Actions
        public Action AnimationComplete { get; set; } = delegate { };
        
        
        // ========================================= Godot Methods
        public override void _Ready() {
            startUpText = "";
            applyText();
            lineDelay = getRandomLineDelay();
        }

        public override void _Process(double Delta_) {
            base._Process(Delta_);
            
            lineTimer += Delta_;
            if (lineTimer >= lineDelay) {
                lineTimer = 0.0;
                
                if (introIndex < INTRO_LINE_LENGTH) {
                    
                    startUpText += generateIntroLine();
                    introIndex++;
                    
                } else if (generationIndex < GENERATION_COUNT) {
                    
                    startUpText += generateTextPart();
                    generationIndex++;
                    
                } else if (outroIndex < OUTRO_LINE_LENGTH) {
                    
                    startUpText += generateOutroLine();
                    outroIndex++;
                    
                } else {
                    
                    AnimationComplete();
                    return;
                    
                }
                
                lineDelay = getRandomLineDelay();
            }
            
            applyText();
        }


        // ========================================= Private Methods
        private double getRandomLineDelay() {
            return LINE_DELAY_BASE + GD.Randfn(-LINE_VARIATION, LINE_VARIATION);
        }
        private void applyText() {
            textLabel.Text = startUpText;
        }
        private string generateIntroLine() {
            return "\n";
        }
        private string generateOutroLine() {
            return "\n";
        }
        private string generateTextPart() {
            string _Return = "";
            
            for (int _I = 0; _I < 6; _I++) {
                float _R = GD.Randf();
                if (_R > 0.75) {
                    _Return += getRandomLineText();
                } else {
                    _Return += generateZeroPaddedHexadecimal();
                }
                _Return += getSpaceText();
            }
            
            return _Return;
        }
        private string generateZeroPaddedHexadecimal() {
            int _RandomNumber = GD.RandRange(0, 999999);
            int _RandomLength = GD.RandRange(4, 22);
            
            string _Return = "0x";
            int _NumberLength = _RandomNumber.ToString().Length;
            if (_NumberLength < _RandomLength) {
                int _ZeroesToAdd = _RandomLength - _NumberLength;
                for (int _I = 0; _I < _ZeroesToAdd; _I++) {
                    _Return += "0";
                }
            }
            _Return += _RandomNumber.ToString();
            
            return _Return;
        }

        private string getRandomLineText() {
            string[] _Lines = [
                "start memory discovery", "initialize hardware", "load kernel", "CPU0 launch EFIO", "CPU1 launch EFIO",
                "CPU0 starting EFIO", "CPU1 starting EFIO", "CPU0 starting EFIO", "CPU1 starting EFIO", "CPU0 starting cell",
                "CPU1 starting cell"
            ];
            return _Lines[GD.Randi() % _Lines.Length];
        }

        private string getSpaceText() {
            string _Return = " ";
            
            float _R0 = GD.Randf();
            if (_R0 > 0.75) {
                _Return += "0 ";
            }
            float _R1 = GD.Randf();
            if (_R1 > 0.75) {
                _Return += "1 ";
            }
            
            return _Return;
        }

    }
    
}


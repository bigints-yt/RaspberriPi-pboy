using Godot;
using System;

namespace RaspberriPipboy.PipboyStartup {
    
    [GlobalClass]
    public partial class PipBoyOSTextController : Control {
        
        [Export] RichTextLabel textLabel;
        [Export] Control exitGrowControl;
        
        private const string OS_TEXT = "************** PIP-OS(R) V7.1.0.8 **************\n\n\nCOPYRIGHT 2075 ROBCO(R)\nLOADER V1.1\nEXEC VERSION 41.10\n64k RAM SYSTEM\n38911 BYTES FREE\nNO HOLOTAPE FOUND\nLOAD ROM(1): DEITRIX 303 BIGINTS EDITION";
        
        private const double CHAR_DELAY = 0.02;
        private const double CARET_DELAY = 0.15;
        
        private const double INTRO_DELAY = 2;
        private const double EXIT_SCROLL_DELAY = 3;
        private const double MOVE_UP_DELAY = 0.03;
        
        private int outroIndex { get; set; } = 0;
        
        private int charIndex { get; set; } = 0;
        private double charTimer { get; set; } = 0.0;
        private string copiedText { get; set; } = "";
        
        private string caretText { get; set; } = "";
        private double caretTimer { get; set; } = 0.0;
        private bool caretVisible { get; set; } = true;
        
        private double introTimer { get; set; } = 0.0;
        private double exitTimer { get; set; } = 0.0;
        private double moveUpTimer { get; set; } = 0.0;
        private float totalMoveUp { get; set; } = 0.0f;
        private const float MOVE_UP_MAX = 1000.0f;

        
        // ========================================= Actions
        public Action AnimationComplete { get; set; } = delegate { };
        
        
        // ========================================= Godot Methods
        public override void _Ready() {
            
        }
        public override void _Process(double Delta_) {
            base._Process(Delta_);
            
            // Intro Timer
            if (introTimer < INTRO_DELAY) {
                introTimer += Delta_;
            } else {
                // Copy Timer
                if (charIndex < OS_TEXT.Length) { // Only copy if there are characters left
                    charTimer += Delta_;
                    if (charTimer >= CHAR_DELAY) {
                        charTimer = 0.0;

                        addNextChar();
                        applyText();
                    }
                } else {
                    exitTimer += Delta_;
                    if (exitTimer >= EXIT_SCROLL_DELAY) {

                        if (totalMoveUp < MOVE_UP_MAX) {
                            moveUpTimer += Delta_;
                            if (moveUpTimer >= MOVE_UP_DELAY) {
                                moveUpTimer = 0.0;
                                moveTextBoxUp();
                            }
                        } else {
                            AnimationComplete();
                            return;
                        }
                    }
                }
            }

            // Caret Timer
            caretTimer += Delta_;
            if (caretTimer >= CARET_DELAY) {
                caretTimer = 0.0;
                caretVisible = !caretVisible;
                updateCaret();
                applyText();
            }
            
        }


        // ========================================= Private Methods
        private void applyText() {
            textLabel.Text = copiedText + caretText;
        }
        private void addNextChar() {
            if (charIndex < OS_TEXT.Length) {
                copiedText += OS_TEXT[charIndex];
                charIndex++;
            }
        }
        private void updateCaret() {
            if (caretVisible) {
                caretText = "\u2588";
            } else {
                caretText = "";
            }
        }
        private void moveTextBoxUp() {
            const float SIZE_CHANGE = 16;
            exitGrowControl.CustomMinimumSize = new Vector2(exitGrowControl.Size.X, exitGrowControl.Size.Y + SIZE_CHANGE);
            totalMoveUp += SIZE_CHANGE;
        }

    }
    
}


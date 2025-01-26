using Godot;
using System;

namespace RaspberriPipboy.PipboyStartup {
    
    [GlobalClass]
    public partial class PipBoyInitController : Control {
        
        [Export] AnimatedSprite2D sprite;
        [Export] Label initLabel;
        
        private const double START_DELAY = 1; 
        private const double EXIT_DELAY = 1; 
        
        private bool animationStarted { get; set; } = false;
        private bool readyToExit { get; set; } = false;
        private double startTimer { get; set; } = 0.0;
        private double exitTimer { get; set; } = 0.0;
        
        
        // ========================================= Actions
        public Action AnimationComplete { get; set; } = delegate { };
        
        
        // ========================================= Godot Methods
        public override void _Ready() {

        }
        public override void _Process(double Delta_) {
            base._Process(Delta_);

            if (startTimer < START_DELAY) {
                startTimer += Delta_;
                return;
            }
            
            if (!animationStarted) {
                animationStarted = true;
                startAnimation();
            }
            
            if (readyToExit) {
                exitTimer += Delta_;
                if (exitTimer >= EXIT_DELAY) {
                    AnimationComplete();
                }
            }
        }
        
        
        // ========================================= Private Methods
        private void startAnimation() {
            sprite.Play();
            sprite.AnimationFinished += onSpriteAnimationFinished;
        }
        private void onSpriteAnimationFinished() {
            sprite.AnimationFinished -= onSpriteAnimationFinished;
            readyToExit = true;
        }
        
    }
    
}
using Godot;

namespace RaspberriPipboy.PipboyStartup {
    
    [GlobalClass]
    public partial class StartUpManager : Node {
        
        [Export] Control sceneRoot;
        [Export] PackedScene startUpTextScrollScene;
        [Export] PackedScene pipBoyOSScene;
        [Export] PackedScene pipBoyInitScene;
        
        private StartUpTextScrollController startUpTextScroll { get; set; }
        private PipBoyOSTextController pipBoyOS { get; set; }
        private PipBoyInitController pipBoyInit { get; set; }
        
        
        // ========================================= Godot Methods
        public override void _Ready() {
            startStartUp();
        }
        
        
        // ========================================= Private Methods
        private void startStartUp() {
            startUpTextScroll = startUpTextScrollScene.Instantiate<StartUpTextScrollController>();
            startUpTextScroll.AnimationComplete += onStartUpComplete;
            sceneRoot.AddChild(startUpTextScroll);
        }
        private void startPipBoyOS() {
            pipBoyOS = pipBoyOSScene.Instantiate<PipBoyOSTextController>();
            pipBoyOS.AnimationComplete += onPipBoyOSComplete;
            sceneRoot.AddChild(pipBoyOS);
        }
        private void startPipBoyInit() {
            pipBoyInit = pipBoyInitScene.Instantiate<PipBoyInitController>();
            pipBoyInit.AnimationComplete += onPipBoyInitComplete;
            sceneRoot.AddChild(pipBoyInit);
        }
        private void onStartUpComplete() {
            startUpTextScroll.QueueFree();
            startUpTextScroll.AnimationComplete -= onStartUpComplete;
            startPipBoyOS();
        }
        private void onPipBoyOSComplete() {
            pipBoyOS.QueueFree();
            pipBoyOS.AnimationComplete -= onPipBoyOSComplete;
            startPipBoyInit();
        }
        private void onPipBoyInitComplete() {
            pipBoyInit.QueueFree();
            pipBoyInit.AnimationComplete -= onPipBoyInitComplete;
        }
        
    }
    
}
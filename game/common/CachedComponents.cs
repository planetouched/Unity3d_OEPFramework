using UnityEngine;

namespace game.common
{
    public static class CachedComponentsExtensions
    {
        public static T GetCachedComponents<T>(this GameObject go) where T : CachedComponents, new()
        {
            var components = new T();
            components.SetGameObject(go);
            return components;
        }

        public static CachedComponents GetCachedComponents(this GameObject go)
        {
            var components = new CachedComponents();
            components.SetGameObject(go);
            return components;
        }
    }

    public class CachedComponents
    {
        private GameObject _go;
        private Transform _transform;
        private RectTransform _rectTransform;
        private Renderer _renderer;
        private SpriteRenderer _spriteRenderer;
        private Camera _camera;
        private Collider _collider;
        private Collider2D _collider2D;
        private Animation _animation;
        private Rigidbody _rigidbody;
        private Rigidbody2D _rigidbody2D;
        private AudioSource _audioSource;
        private ParticleSystem _particleSystem;

        public GameObject gameObject => _go;
        public RectTransform rectTransform => _rectTransform ?? (_rectTransform = _go.GetComponent<RectTransform>());
        public Transform transform => _transform ?? (_transform = _go.transform);
        public Renderer renderer => _renderer ?? (_renderer = _go.GetComponent<Renderer>());
        public SpriteRenderer spriteRenderer => _spriteRenderer ?? (_spriteRenderer = _go.GetComponent<SpriteRenderer>());
        public Camera camera => _camera ?? (_camera = _go.GetComponent<Camera>());
        public Collider collider => _collider ?? (_collider = _go.GetComponent<Collider>());
        public Collider2D collider2D => _collider2D ?? (_collider2D = _go.GetComponent<Collider2D>());
        public Animation animation => _animation ?? (_animation = _go.GetComponent<Animation>());
        public Rigidbody rigidbody => _rigidbody ?? (_rigidbody = _go.GetComponent<Rigidbody>());
        public Rigidbody2D rigidbody2D => _rigidbody2D ?? (_rigidbody2D = _go.GetComponent<Rigidbody2D>());
        public AudioSource audio => _audioSource ?? (_audioSource = _go.GetComponent<AudioSource>());
        public ParticleSystem particleSystem => _particleSystem ?? (_particleSystem = _go.GetComponent<ParticleSystem>());

        public void Reset()
        {
            _transform = null;
            _rectTransform = null;
            _renderer = null;
            _camera = null;
            _collider = null;
            _collider2D = null;
            _animation = null;
            _rigidbody = null;
            _rigidbody2D = null;
            _audioSource = null;
            _particleSystem = null;
            _spriteRenderer = null;
        }
        
        public void SetGameObject(GameObject go)
        {
            _go = go;
        }
    }
}

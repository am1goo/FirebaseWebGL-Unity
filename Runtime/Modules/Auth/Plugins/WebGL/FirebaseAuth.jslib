const firebaseAuthLibrary = {
	$firebaseAuth: {
		sdk: undefined,
		api: undefined,
		callbacks: {},
	
		initialize: function() {
			const plugin = this;
			plugin.firebaseToUnity = window.firebaseToUnity;
			
			if (typeof sdk !== 'undefined') {
				console.error("[Firebase Auth] initialize: already initialized");
				return false;
			}
			
			try {
				plugin.sdk = document.firebaseSdk.auth;
				plugin.api = document.firebaseSdk.authApi;
				console.log('[Firebase Auth] initialize: initialized');
				return true;
			}
			catch(error) {
				console.error(`[Firebase Auth] initialize: failed, error=${error}`);
				return false;
			}
		},
		
		connectAuthEmulator: function(url, options) {
			const plugin = this;
			plugin.api.connectAuthEmulator(plugin.sdk, url, options);
		},
	},
	
	FirebaseWebGL_FirebaseAuth_initialize: function() {
		return firebaseAuth.initialize();
	},
	
	FirebaseWebGL_FirebaseAuth_connectAuthEmulator: function(urlPtr, optionsAsJsonPtr) {
		const url = UTF8ToString(urlPtr);
		if (optionsAsJsonPtr != 0) {
			const optionsAsJson = UTF8ToString(optionsAsJsonPtr);
			const options = JSON.parse(optionsAsJson);
			return firebaseAuth.connectAuthEmulator(url, options);
		}
		else {
			return firebaseAuth.connectAuthEmulator(url);
		}
	},
	
	
};

autoAddDeps(firebaseAuthLibrary, '$firebaseAuth');
mergeInto(LibraryManager.library, firebaseAuthLibrary);
const messagingLibrary = {
	$messaging: {
		sdk: undefined,
		api: undefined,
		callbacks: {},
		
		initialize: function(requestId, callbackPtr) {
			const plugin = this;
			plugin.firebaseToUnity = window.firebaseToUnity;
			
			if (typeof sdk !== 'undefined') {
				plugin.firebaseToUnity(requestId, callbackPtr, false, null, "already initialized");
				return;
			}
			plugin.sdk = document.firebaseSdk.messaging;
			plugin.api = document.firebaseSdk.messagingApi;
			
			console.log(`[Firebase Messaging] initialize: requested`);
			plugin.api.isSupported(plugin.sdk).then(function(success) {
				if (success) {
					if ('serviceWorker' in navigator) {
						navigator.serviceWorker.register('./firebase-messaging-sw.js').then((registration) => {
							console.log(`[Firebase Messaging] initialize: scope=${registration.scope}`);
							plugin.firebaseToUnity(requestId, callbackPtr, true, success, null);
						}).catch((error) => {
							console.error(`[Firebase Messaging] initialize: ${error}`);
							plugin.firebaseToUnity(requestId, callbackPtr, false, null, error);
						});
					}
					else {
						const error = 'Firebase Messaging cannot be registered';
						console.error(`[Firebase Messaging] initialize: ${error}`);
						plugin.firebaseToUnity(requestId, callbackPtr, false, null, error);
					}
				}
				else {
					const error = 'Firebase Messaging is not supported';
					console.error(`[Firebase Messaging] initialize: ${error}`);
					plugin.firebaseToUnity(requestId, callbackPtr, false, null, error);
				}
			}).catch(function(error) {
				console.error(`[Firebase Messaging] initialize: ${error}`);
				plugin.firebaseToUnity(requestId, callbackPtr, false, null, error);
			});
		},
		
		getToken: function(requestId, callbackPtr) {
			const plugin = this;
			plugin.api.getToken(plugin.sdk).then(function(token) {
				console.log(`[Firebase Messaging] getToken: token=${token}`);
				plugin.firebaseToUnity(requestId, callbackPtr, true, token, null);
			}).catch(function(error) {
				console.error(`[Firebase Messaging] getToken: ${error}`);
				plugin.firebaseToUnity(requestId, callbackPtr, false, null, error);
			});
		},
		
		deleteToken: function(requestId, callbackPtr) {
			const plugin = this;
			plugin.api.deleteToken(plugin.sdk).then(function(success) {
				console.log(`[Firebase Messaging] deleteToken: ${(success ? "deleted" : "not deleted")}`);
				plugin.firebaseToUnity(requestId, callbackPtr, true, success, null);
			}).catch(function(error) {
				console.error(`[Firebase Messaging] deleteToken: ${error}`);
				plugin.firebaseToUnity(requestId, callbackPtr, false, null, error);
			});
		},
		
		onMessage: function(instanceId, callbackPtr) {
			const plugin = this;
			if (typeof plugin.callbacks.onMessageUnsubscribe !== 'undefined') {
				plugin.callbacks.onMessageUnsubscribe();
				plugin.callbacks.onMessageUnsubscribe = null;
				console.log('[Firebase Messaging] onMessage: unsubscribed');
			}
			
			if (callbackPtr == 0)
				return;
			
			plugin.callbacks.onMessageUnsubscribe = plugin.api.onMessage(plugin.sdk, function(payload) {
				plugin.firebaseToUnity(instanceId, callbackPtr, true, payload, null);
				console.log(`[Firebase Messaging] onMessage: payload=${payload}`);
			});
			console.log('[Firebase Messaging] onMessage: subscribed');
		},
	},
	
	FirebaseWebGL_FirebaseMessaging_initialize: function(requestId, callbackPtr) {
		messaging.initialize(requestId, callbackPtr);
	},
	
	FirebaseWebGL_FirebaseMessaging_getToken: function(requestId, callbackPtr) {
		messaging.getToken(requestId, callbackPtr);
	},
	
	FirebaseWebGL_FirebaseMessaging_deleteToken: function(requestId, callbackPtr) {
		messaging.deleteToken(requestId, callbackPtr);
	},
	
	FirebaseWebGL_FirebaseMessaging_onMessage: function(instanceId, callbackPtr) {
		messaging.onMessage(instanceId, callbackPtr);
	},
};

const messagingSWLibrary = {
	$messagingSW: {
		sdk: undefined,
		api: undefined,
		callbacks: {},
	
		initialize: function(requestId, callbackPtr) {
			const plugin = this;
			plugin.firebaseToUnity = window.firebaseToUnity;
			
			if (typeof sdk !== 'undefined') {
				plugin.firebaseToUnity(requestId, callbackPtr, false, null, "already initialized");
				return;
			}
			plugin.sdk = document.firebaseSdk.messagingSw;
			plugin.api = document.firebaseSdk.messagingSwApi;
			
			console.log(`[Firebase Messaging Service Worker] initialize: requested`);
			plugin.api.isSupported(plugin.sdk).then(function(success) {
				if (success) {
					console.log(`[Firebase Messaging Service Worker] initialize: initialized`);
					plugin.firebaseToUnity(requestId, callbackPtr, true, success, null);
				}
				else {
					const error = 'Firebase Messaging Service Worker is not supported';
					console.error(`[Firebase Messaging Service Worker] initialize: ${error}`);
					plugin.firebaseToUnity(requestId, callbackPtr, false, null, error);
				}
			}).catch(function(error) {
				console.error(`[Firebase Messaging Service Worker] initialize: ${error}`);
				plugin.firebaseToUnity(requestId, callbackPtr, false, null, error);
			});
		},
		
		experimentalSetDeliveryMetricsExportedToBigQueryEnabled: function(enabled) {
			const plugin = this;
			plugin.api.experimentalSetDeliveryMetricsExportedToBigQueryEnabled(plugin.sdk, enabled);
			console.log(`[Firebase Messaging Service Worker] experimentalSetDeliveryMetricsExportedToBigQueryEnabled: enabled=${enabled}`);
		},
	},
	
	FirebaseWebGL_FirebaseMessaging_ServiceWorker_initialize: function(requestId, callbackPtr) {
		messagingSW.initialize(requestId, callbackPtr);
	},
	
	FirebaseWebGL_FirebaseMessaging_ServiceWorker_experimentalSetDeliveryMetricsExportedToBigQueryEnabled: function(enabled) {
		messagingSW.experimentalSetDeliveryMetricsExportedToBigQueryEnabled(enabled);
	},
};

autoAddDeps(messagingLibrary, '$messaging');
mergeInto(LibraryManager.library, messagingLibrary);

autoAddDeps(messagingSWLibrary, '$messagingSW');
mergeInto(LibraryManager.library, messagingSWLibrary);


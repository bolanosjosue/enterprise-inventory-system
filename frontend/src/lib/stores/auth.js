import { writable } from 'svelte/store';
import { browser } from '$app/environment';

function createAuthStore() {
  const { subscribe, set, update } = writable({
    user: null,
    token: null,
    isAuthenticated: false,
    loading: true
  });

  return {
    subscribe,
    
    init: () => {
      if (browser) {
        const token = localStorage.getItem('token');
        const user = localStorage.getItem('user');
        
        if (token && user) {
          set({
            user: JSON.parse(user),
            token,
            isAuthenticated: true,
            loading: false
          });
        } else {
          set({
            user: null,
            token: null,
            isAuthenticated: false,
            loading: false
          });
        }
      }
    },
    
    login: (user, token) => {
      if (browser) {
        localStorage.setItem('token', token);
        localStorage.setItem('user', JSON.stringify(user));
      }
      set({
        user,
        token,
        isAuthenticated: true,
        loading: false
      });
    },
    
    logout: () => {
      if (browser) {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
      }
      set({
        user: null,
        token: null,
        isAuthenticated: false,
        loading: false
      });
    }
  };
}

export const authStore = createAuthStore();
import { defineStore } from 'pinia';
import api from '../services/api';

export const useAuthStore = defineStore('auth', {
    state: () => ({
        token: localStorage.getItem('token') || null,
        user: JSON.parse(localStorage.getItem('user')) || null
    }),

    getters: {
        isAuthenticated: (state) => !!state.token
    },

    actions: {
        async login(credentials) {
            try {
                const response = await api.post('/Auth/login', credentials);

                const user = response.data.user;
                const userRoles = user.roles || [];

                // ❌ No permitir acceso a admins
                if (userRoles.includes('Admin')) {
                    throw new Error(
                        'Los administradores no pueden acceder a esta aplicación. Por favor use el sistema de administración.'
                    );
                }

                // 🔥 Aquí estaba el error
                // Antes ponía "Cliente", pero tu base usa "Client"
                if (!userRoles.includes('Client')) {
                    throw new Error('Solo los clientes pueden acceder a esta aplicación.');
                }

                // Guardar datos en estado
                this.token = response.data.token;
                this.user = user;

                // Guardar en localStorage para persistencia
                localStorage.setItem('token', this.token);
                localStorage.setItem('user', JSON.stringify(this.user));

                return true;
            } catch (error) {
                console.error('Login failed:', error);
                throw error;
            }
        },

        async register(data) {
            try {
                const response = await api.post('/Auth/register', data);
                return response.data;
            } catch (error) {
                console.error('Registration failed:', error);
                throw error;
            }
        },

        logout() {
            this.token = null;
            this.user = null;

            localStorage.removeItem('token');
            localStorage.removeItem('user');

            window.location.href = '/login';
        }
    }
});

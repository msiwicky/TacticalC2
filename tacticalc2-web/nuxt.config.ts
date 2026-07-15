export default defineNuxtConfig({
	compatibilityDate: "2026-01-01",
	devtools: { enabled: true },
	modules: ["@pinia/nuxt", "@nuxtjs/tailwindcss"],
	runtimeConfig: {
		public: {
			maptilerKey: "",
		},
	},
});
